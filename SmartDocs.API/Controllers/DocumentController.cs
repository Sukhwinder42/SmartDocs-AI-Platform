using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Application.DTOs;
using SmartDocs.Domain.Entities;
using SmartDocs.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace SmartDocs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // UPLOAD DOCUMENT
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Get logged-in user ID
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //// DEBUG START
            //Console.WriteLine("===== CLAIMS =====");

            //foreach (var claim in User.Claims)
            //{
            //    Console.WriteLine($"CLAIM TYPE: {claim.Type}");
            //    Console.WriteLine($"CLAIM VALUE: {claim.Value}");
            //}

            //Console.WriteLine($"Identity Name: {User.Identity?.Name}");
            //Console.WriteLine($"userIdClaim: {userIdClaim}");

            //Console.WriteLine("===== END CLAIMS =====");
            //// DEBUG END
            ///

            var userIdClaim = User.Claims
            .FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier &&
            Guid.TryParse(c.Value, out _))
            ?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user ID.");
            }

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{model.File.FileName}";

            // Upload path
            var uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads");

            // Create folder if not exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Final file path
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Save metadata to database
            var document = new Document
            {
                //UserId = Guid.Parse(userId),
                UserId = userId,
                FileName = uniqueFileName,
                OriginalFileName = model.File.FileName,
                FilePath = filePath,
                ContentType = model.File.ContentType,
                FileSize = model.File.Length,
                UploadedAt = DateTime.UtcNow
            };

            _context.Documents.Add(document);

            await _context.SaveChangesAsync();

            return Ok(new DocumentResponseDto
            {
                Id = document.Id,
                FileName = document.FileName,
                OriginalFileName = document.OriginalFileName,
                UploadedAt = document.UploadedAt
            });
        }

        // GET USER DOCUMENTS
        [HttpGet]
        public IActionResult GetMyDocuments()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userIdClaim = User.Claims
            .FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier &&
            Guid.TryParse(c.Value, out _))
            ?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user ID.");
            }

            //var documents = _context.Documents
            //    .Where(d => d.UserId == Guid.Parse(userId))
            var documents = _context.Documents
            .Where(d => d.UserId == userId)
                .Select(d => new DocumentResponseDto
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    OriginalFileName = d.OriginalFileName,
                    UploadedAt = d.UploadedAt
                })
                .ToList();

            return Ok(documents);
        }
    }
}