using Microsoft.EntityFrameworkCore;

namespace Core
{
    internal class ApplicationContext : DbContext
    {
        internal DbSet<Meme> Memes { get; set; }

        internal ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Memes.db;");
        }
    }
}
