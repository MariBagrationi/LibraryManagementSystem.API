using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.Application.Models.Requests
{
    public class BorrowRecordRequestModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Status status { get; set; } = Status.Borrowed;
    }
}
