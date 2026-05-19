using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FileName { get; set; }

        public string OriginalFileName { get; set; }

        public string FilePath { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        //public string ExtractedText { get; set; }
        public string? ExtractedText { get; set; }

        public bool IsProcessed { get; set; } = false;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ApplicationUser User { get; set; }

        public ICollection<DocumentSummary> Summaries { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }

        public ICollection<DocumentChunk> Chunks { get; set; }
    }
}
