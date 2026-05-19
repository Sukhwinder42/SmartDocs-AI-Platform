using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Domain.Entities
{
    public class DocumentChunk
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public string ChunkText { get; set; }

        public string ?Embedding { get; set; }

        public int ChunkIndex { get; set; }

        // Navigation Property
        public Document Document { get; set; }
    }
}
