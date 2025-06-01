
namespace LibraryManagementSystem.Application.Exceptions.BorrowRecordExceptions
{
    public class BorrowRecordAlreadyExistsEx : Exception
    {
        public static string Code { get; private set; } = "BorrowrecordAlreadyExists";
        public BorrowRecordAlreadyExistsEx(string message) : base(message) { }
    }
}
