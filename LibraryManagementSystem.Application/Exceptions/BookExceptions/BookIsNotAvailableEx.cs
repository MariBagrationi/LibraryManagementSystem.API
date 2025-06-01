
namespace LibraryManagementSystem.Application.Exceptions.BookExceptions
{
    public class BookIsNotAvailableEx : Exception
    {
        public static string Code { get; private set; } = "BookIsNotAvailable";
        public BookIsNotAvailableEx(string message) : base(message) { }
    }
}
