using LibraryManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Persistance.Context
{
    public class LibraryManagementContext : DbContext
    {
        public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options) : base (options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically apply all IEntityTypeConfiguration<> classes
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryManagementContext).Assembly);
        }
    }
}
