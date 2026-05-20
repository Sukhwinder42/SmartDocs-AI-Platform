using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pgvector;

namespace SmartDocs.Domain.Entities
{
    public class DocumentEmbedding
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public string ChunkText { get; set; }

        public Vector Embedding { get; set; }
    }
}
