
namespace LibraryManagementSystem.Application.Exceptions.BookExceptions
{
    public class BookAlreadyExistsEx : Exception
    {
        public static string Code { get; private set; } = "BookAlreadyExists";
        public BookAlreadyExistsEx(string message) : base(message) { }
        
    }
}
