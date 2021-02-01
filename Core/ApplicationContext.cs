using Microsoft.EntityFrameworkCore;

namespace Core
{
    internal class ApplicationContext : DbContext
    {
        internal DbSet<Image> Images { get; set; }

        internal ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Images.db;");
        }
    }
}
