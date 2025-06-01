using LibraryManagementSystem.Application.Exceptions.AuthorExceptions;
using LibraryManagementSystem.Application.Exceptions.BookExceptions;
using LibraryManagementSystem.Application.Exceptions.BorrowRecordExceptions;
using LibraryManagementSystem.Application.Exceptions.PatronExceptions;
using LibraryManagementSystem.Application.Exceptions.UserExceptions;
using Serilog;

namespace LibraryManagementSystem.API.Infrastructure.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        
        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await Handle(context, ex);
            }
        }

        public async Task Handle(HttpContext context, Exception ex)
        {

            int statusCode = ex switch
            {
                AuthorAlreadyExistsEx => StatusCodes.Status409Conflict,
                AuthorDoesNotExistEx => StatusCodes.Status404NotFound,

                BookAlreadyExistsEx => StatusCodes.Status409Conflict,
                BookDoesNotExistEx => StatusCodes.Status404NotFound,

                BorrowRecordAlreadyExistsEx => StatusCodes.Status409Conflict,
                BorrowRecordDoesNotExistEx => StatusCodes.Status404NotFound,

                PatronAlreadyExistsEx => StatusCodes.Status409Conflict,
                PatronDoesNotExistEx => StatusCodes.Status404NotFound,

                UserAreadyExistsEx => StatusCodes.Status409Conflict,

                _ when ex is ArgumentNullException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            Log.Error($"An error occurred: {ex.Message}");
            
            var response = new
            {
                StatusCode = statusCode,
                Details = ex.StackTrace, //dev only
                Message = ex.Message 
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
