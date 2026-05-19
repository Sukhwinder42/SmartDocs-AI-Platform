using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pgvector;
using SmartDocs.Application.DTOs;
using SmartDocs.Application.Interfaces;
using SmartDocs.Application.Interfaces;
using SmartDocs.Domain.Entities;
using SmartDocs.Infrastructure.Data;


namespace SmartDocs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOcrService _ocrService;
        private readonly IGeminiService _geminiService;
        private readonly ITextChunkService _textChunkService;
        private readonly VectorDbContext _vectorContext;
        private readonly IVectorSearchService _vectorSearchService;
        private readonly ITextCleanerService _textCleanerService;

        public AiController(
            ApplicationDbContext context,
            IOcrService ocrService,
            IGeminiService geminiService,
            ITextChunkService textChunkService,
            VectorDbContext vectorContext,
            IVectorSearchService vectorSearchService,
            ITextCleanerService textCleanerService)
        {
            _context = context;
            _ocrService = ocrService;
            _geminiService = geminiService;
            _textChunkService = textChunkService;
            _vectorContext = vectorContext;
            _vectorSearchService = vectorSearchService;
            _textCleanerService = textCleanerService;
        }

        // =========================
        // SUMMARIZE DOCUMENT
        // =========================
        [HttpPost("summarize/{documentId}")]
        public async Task<IActionResult> SummarizeDocument(Guid documentId)
        {
            var document = await _context.Documents.FindAsync(documentId);

            if (document == null)
                return NotFound("Document not found.");

            // 1. OCR
            var extractedText = await _ocrService.ExtractTextAsync(document.FilePath);

            extractedText = Clean(extractedText);

            document.ExtractedText = extractedText;

            // 2. Chunking
            var chunks = _textChunkService.SplitIntoChunks(extractedText);

            int index = 0;

            foreach (var chunk in chunks)
            {
                var safeChunk = Clean(chunk);

                // Save chunk
                _context.DocumentChunks.Add(new DocumentChunk
                {
                    DocumentId = document.Id,
                    ChunkText = safeChunk,
                    ChunkIndex = index++
                });

                // Embedding
                var embedding = await _geminiService.GenerateEmbeddingAsync(safeChunk);

                _vectorContext.DocumentEmbeddings.Add(new DocumentEmbedding
                {
                    DocumentId = document.Id,
                    ChunkText = safeChunk,
                    Embedding = new Vector(embedding)
                });
            }

            // 3. AI Summary
            var summary = await _geminiService.GenerateSummaryAsync(extractedText);
            summary = Clean(summary);

            _context.DocumentSummaries.Add(new DocumentSummary
            {
                DocumentId = document.Id,
                SummaryText = summary
            });

            await _context.SaveChangesAsync();
            await _vectorContext.SaveChangesAsync();

            return Ok(new
            {
                ExtractedText = extractedText,
                Summary = summary
            });
        }

        // =========================
        // ASK QUESTION (RAG)
        // =========================
        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion(AskQuestionDto model)
        {
            var document = await _context.Documents.FindAsync(model.DocumentId);

            if (document == null)
                return NotFound("Document not found.");

            var chunks = await _vectorSearchService
                .SearchRelevantChunksAsync(model.DocumentId, model.Question);

            var combinedText = Clean(string.Join("\n", chunks));
            var question = Clean(model.Question);

            var answer = await _geminiService.AskQuestionAsync(combinedText, question);
            answer = Clean(answer);

            _context.ChatMessages.Add(new ChatMessage
            {
                DocumentId = model.DocumentId,
                UserQuestion = question,
                AIResponse = answer
            });

            await _context.SaveChangesAsync();

            return Ok(new ChatResponseDto
            {
                Question = question,
                Answer = answer
            });
        }

        // =========================
        // SAFE TEXT CLEANER
        // =========================
        private string Clean(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input
                .Replace("\0", "")   // 🔥 FIX PostgreSQL crash
                .Trim();
        }

        [HttpGet("history/{documentId}")]
        public async Task<IActionResult> GetChatHistory(Guid documentId)
        {
            var chats = _context.ChatMessages
                .Where(x => x.DocumentId == documentId)
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    user = x.UserQuestion,
                    ai = x.AIResponse
                })
                .ToList();

            return Ok(chats);
        }

        [HttpGet("dashboard/stats")]
        public IActionResult GetDashboardStats()
        {
            var documents = _context.Documents.Count();
            var chats = _context.ChatMessages.Count();
            var summaries = _context.DocumentSummaries.Count();

            return Ok(new
            {
                documents,
                chats,
                summaries
            });
        }

    }
}