using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDocs.Infrastructure.Data
{
    public class VectorDbContextFactory
        : IDesignTimeDbContextFactory<VectorDbContext>
    {
        public VectorDbContext CreateDbContext(string[] args)
        {
            var connectionString =
               Environment.GetEnvironmentVariable(
                   "ConnectionStrings__DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception(
                    "Connection string not found in environment variables.");
            }

            var optionsBuilder =
                new DbContextOptionsBuilder<VectorDbContext>();

            optionsBuilder.UseNpgsql(
                connectionString,
                o => o.UseVector());

            return new VectorDbContext(optionsBuilder.Options);
        }
    }
}
