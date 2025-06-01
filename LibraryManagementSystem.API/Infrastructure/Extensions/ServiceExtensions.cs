using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Application.Services.Authors;
using LibraryManagementSystem.Application.Services.Books;
using LibraryManagementSystem.Application.Services.BorrowRecords;
using LibraryManagementSystem.Application.Services.Patrons;
using LibraryManagementSystem.Application.Services.Users;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Infrastructure;
using PersonManagement.Infrastructure.Users;

namespace LibraryManagementSystem.API.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService,  BookService>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddScoped<IPatronService, PatronService>();
            services.AddScoped<IPatronRepository, PatronRepository>();

            services.AddScoped<IBorrowRecordService, BorrowRecordService>();
            services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        }
    }
}
