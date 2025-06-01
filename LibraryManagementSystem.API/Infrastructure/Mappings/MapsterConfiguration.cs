using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Models.User;
using LibraryManagementSystem.Domain.Models;
using Mapster;

namespace LibraryManagementSystem.API.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection service)
        {

            TypeAdapterConfig<Book, BookResponseModel>.NewConfig();
            TypeAdapterConfig<BookRequestModel, Book>.NewConfig();

            TypeAdapterConfig<Author, AuthorResponseModel>.NewConfig();
            TypeAdapterConfig<AuthorRequestModel, Author>.NewConfig();

            TypeAdapterConfig<BorrowRecord, BorrowRecordResponseModel>.NewConfig();
            TypeAdapterConfig<BorrowRecordRequestModel, Author>.NewConfig();

            TypeAdapterConfig<Patron, AuthorResponseModel>.NewConfig();
            TypeAdapterConfig<PatronRequestModel, Author>.NewConfig();

            TypeAdapterConfig<UserRegisterModel, User>.NewConfig()
                                                      .Map(dest => dest.Role, src => ParseRole(src.Role));

            TypeAdapterConfig<PagedResult<Book>, PagedResult<BookResponseModel>>.NewConfig()
                .Map(dest => dest.Items, src => src.Items.Adapt<List<BookResponseModel>>());

            TypeAdapterConfig<PagedResult<Author>, PagedResult<AuthorResponseModel>>.NewConfig()
                .Map(dest => dest.Items, src => src.Items.Adapt<List<AuthorResponseModel>>());

            TypeAdapterConfig<PagedResult<BorrowRecord>, PagedResult<BorrowRecordResponseModel>>.NewConfig()
                .Map(dest => dest.Items, src => src.Items.Adapt<List<BorrowRecordResponseModel>>());

            TypeAdapterConfig<PagedResult<Patron>, PagedResult<PatronResponseModel>>.NewConfig()
                .Map(dest => dest.Items, src => src.Items.Adapt<List<PatronResponseModel>>());
        }

        private static Role ParseRole(string role)
        {
            return Enum.TryParse<Role>(role, true, out var parsedRole) ? parsedRole : Role.User;
        }
    }
}
