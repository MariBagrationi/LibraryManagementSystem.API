
namespace LibraryManagementSystem.Domain.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate {  get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Status status { get; set; } = Status.Borrowed;

        public Book Book { get; set; } = new Book();
        public Patron Patron { get; set; } = new Patron();
    }

    public enum Status
    {
        Borrowed,
        Returned,
        Overdue
    }
}
