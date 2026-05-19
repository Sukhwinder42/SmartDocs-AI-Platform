using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartDocs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Infrastructure.Data
{
    //internal class ApplicationDbContext
    //{
    //}
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentSummary> DocumentSummaries { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<DocumentChunk> DocumentChunks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Document → User
            builder.Entity<Document>()
                .HasOne(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Summary → Document
            builder.Entity<DocumentSummary>()
                .HasOne(s => s.Document)
                .WithMany(d => d.Summaries)
                .HasForeignKey(s => s.DocumentId);

            // Chat → Document
            builder.Entity<ChatMessage>()
                .HasOne(c => c.Document)
                .WithMany(d => d.ChatMessages)
                .HasForeignKey(c => c.DocumentId);

            // Chunk → Document
            builder.Entity<DocumentChunk>()
                .HasOne(c => c.Document)
                .WithMany(d => d.Chunks)
                .HasForeignKey(c => c.DocumentId);
        }
    }
}
