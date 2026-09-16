using Microsoft.EntityFrameworkCore;

namespace CarGallery
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=CarGalleryDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}