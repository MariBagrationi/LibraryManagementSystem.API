using Asp.Versioning;
using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Services.Authors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/v{version:apiVersion}/authors")]
    //[ApiVersion("1.0")] 
    public class AuthorsController : ControllerBase
    {
        IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Retrieves all authors with pagination.
        /// </summary>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paged list of authors.</returns>
        [HttpGet]
        [SwaggerResponse(200, "List of authors", typeof(PagedResult<AuthorResponseModel>))]
        public async Task<ActionResult<PagedResult<AuthorResponseModel>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var authors = await _authorService.GetAll(page, pageSize, cancellationToken).ConfigureAwait(false);
            return Ok(authors);
        }

        /// <summary>
        /// Retrieves a specific author by ID.
        /// </summary>
        /// <param name="id">Author ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Author details.</returns>
        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [SwaggerResponse(200, "Author found", typeof(AuthorResponseModel))]
        [SwaggerResponse(404, "Author not found")]
        public async Task<ActionResult<AuthorResponseModel>> Get(int id, CancellationToken cancellationToken)
        {
            var author = await _authorService.Get(id, cancellationToken).ConfigureAwait(false);
            if (author == null)
                return NotFound();

            return Ok(author);
        }


        /// <summary>
        /// Retrieves all books written by a specific author.
        /// </summary>
        /// <param name="id">Author ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of books.</returns>
        [HttpGet("{id}/books")]
        [SwaggerResponse(200, "List of books", typeof(List<BookResponseModel>))]
        [SwaggerResponse(404, "Author not found")]
        public async Task<ActionResult<List<BookResponseModel>>> GetBooksByAuthor(int id, CancellationToken cancellationToken)
        {
            var books = await _authorService.GetBooksByAuthor(id, cancellationToken);
            if (books == null || books.Count == 0)
                return NotFound("No books found for this author.");

            return Ok(books);
        }


        /// <summary>
        /// Adds a new author.
        /// </summary>
        /// <param name="author">Author request model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Created author.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(201, "Author created", typeof(AuthorResponseModel))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Post([FromBody] AuthorRequestModel author, CancellationToken cancellationToken)
        {
            var createdAuthor = await _authorService.Create(author, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = createdAuthor.Id }, createdAuthor);
        }

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="author">Updated author data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(204, "Author updated successfully")]
        [SwaggerResponse(404, "Author not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Put([FromBody] AuthorRequestModel author, CancellationToken cancellationToken)
        {
            await _authorService.Update(author, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes an author by ID.
        /// </summary>
        /// <param name="id">Author ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(204, "Author deleted successfully")]
        [SwaggerResponse(404, "Author not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _authorService.Delete(id, cancellationToken);
            return NoContent();
        }
    }
}
