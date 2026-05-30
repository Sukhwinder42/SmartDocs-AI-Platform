using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using SmartDocs.Application.Interfaces;
using SmartDocs.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Infrastructure.Services
{
    public class VectorSearchService : IVectorSearchService
    {
        private readonly VectorDbContext _vectorContext;
        private readonly IGeminiService _geminiService;

        public VectorSearchService(
            VectorDbContext vectorContext, IGeminiService geminiService)
        {
            _vectorContext = vectorContext;
            _geminiService = geminiService;
        }

        //public async Task<List<string>> SearchRelevantChunksAsync(
        //    Guid documentId,
        //    string question)
        //{
        //    // TEMP BASIC SEARCH
        //    // Later replace with cosine similarity

        //    return await _vectorContext.DocumentEmbeddings
        //        .Where(x => x.DocumentId == documentId)
        //        .Take(5)
        //        .Select(x => x.ChunkText)
        //        .ToListAsync();
        //}

        public async Task<List<string>> SearchRelevantChunksAsync(
        Guid documentId,
        string question)
        {
            // Generate embedding for the user's question
            var questionEmbedding =
                await _geminiService.GenerateEmbeddingAsync(question);

            // Convert float[] to pgvector Vector
            var vector = new Vector(questionEmbedding);

            // Semantic similarity search
            var chunks = await _vectorContext.DocumentEmbeddings
                .Where(x => x.DocumentId == documentId)
                .OrderBy(x => x.Embedding.CosineDistance(vector))
                .Take(5)
                .Select(x => x.ChunkText)
                .ToListAsync();

            return chunks;
        }
    }
}
