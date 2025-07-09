using Microsoft.EntityFrameworkCore;
using BackendTob.Models; // pastikan namespace Models kamu benar

namespace BackendTob.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Policy> Policies { get; set; }
    }
}
