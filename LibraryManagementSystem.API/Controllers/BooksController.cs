using Asp.Versioning;
using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Services.Books;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    //[AllowAnonymous]
    //[ApiVersion("1.0")] // Version 1
    //[ApiVersion("2.0")] // Version 2 
    public class BooksController : ControllerBase
    {
        IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }


        /// <summary>
        /// Retrieves all books with pagination.
        /// </summary>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paged list of books.</returns>
        [HttpGet]
        [SwaggerResponse(200, "List of books", typeof(PagedResult<BookResponseModel>))]
        public async Task<ActionResult<PagedResult<BookResponseModel>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            Log.Information("Fetching all books. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            return Ok(await _bookService.GetAll(page, pageSize, cancellationToken));
        }

        /// <summary>
        /// Retrieves a specific book by ID.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Book details.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Book found", typeof(BookResponseModel))]
        [SwaggerResponse(404, "Book not found")]
        public async Task<ActionResult<BookResponseModel>> Get(int id, CancellationToken cancellationToken)
        {
            Log.Information("Fetching book with ID: {BookId}", id);
            var book = await _bookService.Get(id, cancellationToken);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        /// <summary>
        /// Searches books by title or author.
        /// </summary>
        /// <param name="title">Book title (optional).</param>
        /// <param name="author">Author name (optional).</param>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Matching books.</returns>
        [HttpGet("search")]
        [SwaggerResponse(200, "List of books", typeof(PagedResult<BookResponseModel>))]
        [SwaggerResponse(400, "At least one search parameter (title or author) must be provided.")]
        public async Task<IActionResult> SearchBooks([FromQuery] string title, [FromQuery] string author, CancellationToken cancellationToken)
        {
            Log.Information("Searching books. Title: {Title}, Author: {Author}", title, author);
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(author))
            {
                Log.Information("Searching failed");
                return BadRequest("At least one search parameter: title or author must be provided.");
            }

            var books = await _bookService.SearchBooks(title, author, cancellationToken);
            return Ok(books);
        }

        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">Book request model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Created book.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        [SwaggerResponse(201, "Book created", typeof(BookResponseModel))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> Post([FromBody] BookRequestModel book, CancellationToken cancellationToken)
        {
            Log.Information("Adding new book: {BookTitle}", book.Title);
            var createdBook = await _bookService.Create(book, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = createdBook.Id }, createdBook);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="book">Updated book data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        [SwaggerResponse(204, "Book updated successfully")]
        [SwaggerResponse(404, "Book not found")]
        public async Task<IActionResult> Put([FromBody] BookRequestModel book, CancellationToken cancellationToken)
        {
            Log.Information("Updating book with ID: {BookId}", book.Id);
            await _bookService.Update(book, cancellationToken);
            return NoContent();
        }


        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        [SwaggerResponse(204, "Book deleted successfully")]
        [SwaggerResponse(404, "Book not found")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            Log.Information("Deleting book with ID: {BookId}", id);
            await _bookService.Delete(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Checks if a book is available for borrowing.
        /// </summary>
        /// <param name="id">Book ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Boolean indicating availability.</returns>
        [HttpGet("{id}/availability")]
        [SwaggerResponse(200, "Book availability status", typeof(bool))]
        public async Task<ActionResult<bool>> CheckAvailability(int id, CancellationToken cancellationToken)
        {
            Log.Information("Checking availability for book ID: {BookId}", id);
            var isAvailable = await _bookService.CheckAvailability(id, cancellationToken);
            return Ok(isAvailable);
        }

    }
}
