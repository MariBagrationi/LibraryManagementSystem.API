using LibraryManagementSystem.Persistance.Context;
using LibraryManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LibraryManagementSystem.Persistance.Seed
{
    public static class LibraryManagementSeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LibraryManagementContext>();
            
            try
            {
                await MigrateDatabaseAsync(context);
                await SeedDataAsync(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while seeding the database.");
            }
        }

        private static async Task MigrateDatabaseAsync(LibraryManagementContext context)
        {
            //logger.LogInformation("Applying database migrations...");
            Log.Information("Applying database migrations...");
            await context.Database.MigrateAsync();
            //logger.LogInformation("Database migrations applied.");
            Log.Information("Database migrations applied.");
        }

        private static async Task SeedDataAsync(LibraryManagementContext context)
        {
            bool isSeeded = false;

            isSeeded |= await SeedAuthorsAsync(context);
            isSeeded |= await SeedBooksAsync(context);
            isSeeded |= await SeedPatronsAsync(context);

            if (isSeeded)
            {
                await context.SaveChangesAsync();
                Log.Information("Database seeding completed.");
                //logger.LogInformation("Database seeding completed.");
            }
            else
            {
                //logger.LogInformation("No new data to seed.");
                Log.Information("No new data to seed.");
            }
        }
        //seeding library * todo
        private static async Task<bool> SeedAuthorsAsync(LibraryManagementContext context)
        {
            var authors = new List<Author>
            {
                new() { FirstName = "George", LastName = "Orwell", Biography = "Author of 1984", DateOfBirth = new DateTime(1903, 6, 25) },
                new() { FirstName = "Jane", LastName = "Austen", Biography = "Author of Pride and Prejudice", DateOfBirth = new DateTime(1775, 12, 16) }
            };

            if (await context.Authors.AnyAsync())
            {
                return false; // Authors already exist, no need to seed
            }

            await context.Authors.AddRangeAsync(authors);
            //logger.LogInformation("Seeded authors.");
            Log.Information("Seeded authors.");
            return true;
        }

        private static async Task<bool> SeedBooksAsync(LibraryManagementContext context)
        {
            var books = new List<Book>
            {
                new() { Title = "1984", ISBN = "123456789", PublicationYear = 1949, Description = "Dystopian novel", CoverImageUrl = "", Quantity = 5, AuthorId = 1 },
                new() { Title = "Pride and Prejudice", ISBN = "987654321", PublicationYear = 1813, Description = "Romantic novel", CoverImageUrl = "", Quantity = 3, AuthorId = 2 }
            };

            if (await context.Books.AnyAsync())
            {
                return false; // Books already exist, no need to seed
            }

            await context.Books.AddRangeAsync(books);
            //logger.LogInformation("Seeded books.");
            Log.Information("Seeded books.");
            return true;
        }

        private static async Task<bool> SeedPatronsAsync(LibraryManagementContext context)
        {
            var patrons = new List<Patron>
            {
                new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", MembershipDate = DateTime.UtcNow },
                new() { FirstName = "Emily", LastName = "Smith", Email = "emily.smith@example.com", MembershipDate = DateTime.UtcNow }
            };

            if (await context.Patrons.AnyAsync())
            {
                return false; // Patrons already exist, no need to seed
            }

            await context.Patrons.AddRangeAsync(patrons);
            //logger.LogInformation("Seeded patrons.");
            Log.Information("Seeded patrons.");
            return true;
        }
    }
}
