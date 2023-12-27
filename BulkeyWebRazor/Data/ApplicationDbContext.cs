using BulkeyWebRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkeyWebRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1,name="Rahul",displayOder=2 },
                new Category { Id=2,name="Kisan",displayOder=5},
                new Category { Id = 3, name = "Kisan", displayOder = 7 }
                );
        }
    }
}
