using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Domain.Entities
{
    public class DocumentSummary
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public string SummaryText { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public Document Document { get; set; }
    }
}
