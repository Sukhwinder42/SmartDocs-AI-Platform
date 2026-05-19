using Microsoft.EntityFrameworkCore;
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

        public VectorSearchService(
            VectorDbContext vectorContext)
        {
            _vectorContext = vectorContext;
        }

        public async Task<List<string>> SearchRelevantChunksAsync(
            Guid documentId,
            string question)
        {
            // TEMP BASIC SEARCH
            // Later replace with cosine similarity

            return await _vectorContext.DocumentEmbeddings
                .Where(x => x.DocumentId == documentId)
                .Take(5)
                .Select(x => x.ChunkText)
                .ToListAsync();
        }
    }
}
