using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public string UserQuestion { get; set; }

        public string AIResponse { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public Document Document { get; set; }
    }
}
