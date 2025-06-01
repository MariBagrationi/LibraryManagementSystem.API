using FluentValidation;
using LibraryManagementSystem.Application.Models.Requests;

namespace LibraryManagementSystem.API.Infrastructure.Validators.AuthorValidators
{
    public class AuthorRequestModelValidator : AbstractValidator<AuthorRequestModel>
    {
        public AuthorRequestModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Author FirstName is required")
                .MaximumLength(50)
                .WithMessage("Too long First Name");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Author LastName is required")
                .MaximumLength(50)
                .WithMessage("Too long Last Name");
        }
    }
}
