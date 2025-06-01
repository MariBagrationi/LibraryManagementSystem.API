using Asp.Versioning;
using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Services.Patrons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [Authorize(Roles = "Admin")]
    //[ApiVersion("1.0")] 
    public class PatronsController : ControllerBase
    {
        IPatronService _patronService;
        public PatronsController(IPatronService patronService)
        {
            _patronService = patronService;
        }

        /// <summary>
        /// Retrieves all patrons with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1).</param>
        /// <param name="pageSize">Number of items per page (default: 10).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paged list of patrons.</returns>
        [HttpGet]
        [SwaggerResponse(200, "List of patrons", typeof(PagedResult<PatronResponseModel>))]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<ActionResult<PagedResult<PatronResponseModel>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var patrons = await _patronService.GetAll(pageNumber, pageSize, cancellationToken);
            return Ok(patrons);
        }

        /// <summary>
        /// Retrieves a specific patron by ID.
        /// </summary>
        /// <param name="id">Patron ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Patron details.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Patron found", typeof(PatronResponseModel))]
        [SwaggerResponse(404, "Patron not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<ActionResult<PatronResponseModel>> Get(int id, CancellationToken cancellationToken)
        {
            var patron = await _patronService.Get(id, cancellationToken);
            if (patron == null)
                return NotFound();

            return Ok(patron);
        }

        /// <summary>
        /// Retrieves all books borrowed by a specific patron.
        /// </summary>
        /// <param name="id">Patron ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of books.</returns>
        [HttpGet("{id}/books")]
        [SwaggerResponse(200, "List of books", typeof(List<BookResponseModel>))]
        [SwaggerResponse(404, "Patron not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<ActionResult<List<BookResponseModel>>> GetAllBooksById(int id, CancellationToken cancellationToken)
        {
            var books = await _patronService.GetAllBooksbyId(id, cancellationToken);
            if (books == null || books.Count == 0)
                return NotFound("No books found for this patron.");

            return Ok(books);
        }

        /// <summary>
        /// Adds a new patron.
        /// </summary>
        /// <param name="patron">Patron request model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Created patron.</returns>
        [HttpPost]
        [SwaggerResponse(201, "Patron created", typeof(PatronResponseModel))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Post([FromBody] PatronRequestModel patron, CancellationToken cancellationToken)
        {
            var createdPatron = await _patronService.Create(patron, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = createdPatron.Id }, createdPatron);
        }


        /// <summary>
        /// Updates an existing patron.
        /// </summary>
        /// <param name="patron">Updated patron data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [SwaggerResponse(204, "Patron updated successfully")]
        [SwaggerResponse(404, "Patron not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Put([FromBody] PatronRequestModel patron, CancellationToken cancellationToken)
        {
            await _patronService.Update(patron, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a patron by ID.
        /// </summary>
        /// <param name="id">Patron ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Patron deleted successfully")]
        [SwaggerResponse(404, "Patron not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _patronService.Delete(id, cancellationToken);
            return NoContent();
        }
    }
}
