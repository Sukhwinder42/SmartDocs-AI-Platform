using Microsoft.EntityFrameworkCore;
using SmartDocs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Infrastructure.Data
{
    public class VectorDbContext : DbContext
    {
        public VectorDbContext(
            DbContextOptions<VectorDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocumentEmbedding> DocumentEmbeddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<DocumentEmbedding>()
                .Property(x => x.Embedding)
                //.HasColumnType("vector(768)");
                .HasColumnType("vector(3072)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
