using Asp.Versioning;
using LibraryManagementSystem.API.Infrastructure.Validators.BorrowRecordValidators;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Services.BorrowRecords;
using LibraryManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [Authorize]
    //[ApiVersion("1.0")] 
    //[ApiVersion("2.0")]
    public class BorrowRecordsController : ControllerBase
    {
        IBorrowRecordService _borrowRecordService;
        public BorrowRecordsController(IBorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }

        /// <summary>
        /// Retrieves all borrow records with optional filtering.
        /// </summary>
        /// <param name="userId">Filter by user ID (optional).</param>
        /// <param name="bookId">Filter by book ID (optional).</param>
        /// <param name="isReturned">Filter by return status (true for returned, false for not returned) (optional).</param>
        /// <param name="borrowedAfter">Filter by borrow date (records borrowed after this date) (optional).</param>
        /// <param name="borrowedBefore">Filter by borrow date (records borrowed before this date) (optional).</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of borrow records matching the filter criteria.</returns>
        [HttpGet]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        [SwaggerResponse(200, "List of borrow records", typeof(List<BorrowRecordResponseModel>))]
        public async Task<ActionResult<List<BorrowRecordResponseModel>>> GetAll(
                    [FromQuery] int? patronId = null,
                    [FromQuery] int? bookId = null,
                    [FromQuery] Status? isReturned = null,
                    [FromQuery] DateTime? borrowedAfter = null,
                    [FromQuery] DateTime? borrowedBefore = null,
                    CancellationToken cancellationToken = default)
        {
            var records = await _borrowRecordService.GetAll(patronId, bookId, isReturned, borrowedAfter, borrowedBefore, cancellationToken);
            return Ok(records);
        }


        /// <summary>
        /// Retrieves a specific borrow record by ID.
        /// </summary>
        /// <param name="id">Borrow record ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Borrow record details.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        [SwaggerResponse(200, "Borrow record found", typeof(BorrowRecordResponseModel))]
        [SwaggerResponse(404, "Borrow record not found")]
        public async Task<ActionResult<BorrowRecordResponseModel>> Get(int id, CancellationToken cancellationToken)
        {
            var record = await _borrowRecordService.Get(id, cancellationToken);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        /// <summary>
        /// Retrieves all overdue borrow records.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of overdue borrow records.</returns>
        [HttpGet("overdue")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        [SwaggerResponse(200, "List of overdue borrow records", typeof(List<BookResponseModel>))]
        public async Task<ActionResult<List<BookResponseModel>>> GetOverdueBooks(CancellationToken cancellationToken)
        {
            var overdueBooks = await _borrowRecordService.GetOverdueBooks(cancellationToken);
            return Ok(overdueBooks);
        }

        /// <summary>
        /// Creates a new borrow record (checks out a book).
        /// </summary>
        /// <param name="borrowRecord">Borrow record request model.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Created borrow record.</returns>
        [HttpPost]
        [SwaggerResponse(201, "Borrow record created", typeof(BorrowRecordResponseModel))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Post([FromBody] BorrowRecordRequestModel borrowRecord, CancellationToken cancellationToken)
        {
            var createdRecord = await _borrowRecordService.Create(borrowRecord, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = createdRecord.Id }, createdRecord);
        }

        /// <summary>
        /// Returns a borrowed book.
        /// </summary>
        /// <param name="id">Borrow record ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Returned book details.</returns>
        [HttpPut("{id}/return")]
        [SwaggerResponse(200, "Book returned successfully", typeof(BookResponseModel))]
        [SwaggerResponse(404, "Borrow record not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<ActionResult<BookResponseModel>> ReturnBook(int id, CancellationToken cancellationToken)
        {
            await _borrowRecordService.ReturnBook(id, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        /// <summary>
        /// Deletes a borrow record by ID.
        /// </summary>
        /// <param name="id">Borrow record ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Borrow record deleted successfully")]
        [SwaggerResponse(404, "Borrow record not found")]
        [SwaggerResponse(403, "Forbidden: Admins only")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _borrowRecordService.Delete(id, cancellationToken);
            return NoContent();
        }
    }
}
