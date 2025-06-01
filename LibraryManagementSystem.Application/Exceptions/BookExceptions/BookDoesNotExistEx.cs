
namespace LibraryManagementSystem.Application.Exceptions.BookExceptions
{
    public class BookDoesNotExistEx : Exception
    {
        public static string Code { get; private set; } = "BookDoesNotExist";
        public BookDoesNotExistEx(string message) : base(message) { }
      
    }
}
