
namespace LibraryManagementSystem.Application.Exceptions.BorrowRecordExceptions
{
    public class BorrowRecordDoesNotExistEx : Exception
    {
        public static string Code { get; private set; } = "BorrowrecordDoesNotExist";
        public BorrowRecordDoesNotExistEx(string message) : base(message) { }
    }
}
