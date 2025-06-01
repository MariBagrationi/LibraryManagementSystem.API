using FluentValidation;
using LibraryManagementSystem.Application.Models.Requests;

namespace LibraryManagementSystem.API.Infrastructure.Validators.BookValidators
{
    public class BookRequestModelValidator : AbstractValidator<BookRequestModel>
    {
        public BookRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(100)
                .WithMessage("Title is too long");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.ISBN)
                .NotEmpty()
                .WithMessage("ISBN is required")
                .Length(13)
                .WithMessage("ISBN length must be 13");

            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("Author ID is required")
                .GreaterThan(0).WithMessage("Valid Author ID is required");

            RuleFor(x => x.PublicationYear)
                .InclusiveBetween(1500, 2025).WithMessage("Published year must be between 1500 and 2100");

        }
    }
}
