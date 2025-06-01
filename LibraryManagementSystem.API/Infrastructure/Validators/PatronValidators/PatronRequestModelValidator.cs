using FluentValidation;
using LibraryManagementSystem.Application.Models.Requests;

namespace LibraryManagementSystem.API.Infrastructure.Validators.PatronValidators
{
    public class PatronRequestModelValidator : AbstractValidator<PatronRequestModel>
    {
        public PatronRequestModelValidator()
        {
            RuleFor(x => x.FirstName)
               .NotEmpty()
               .WithMessage("Patron FirstName is required")
               .MaximumLength(50)
               .WithMessage("Too long First Name");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Patron LastName is required")
                .MaximumLength(50)
                .WithMessage("Too long Last Name");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Gmail is required")
                .EmailAddress()
                .WithMessage("Gmail should be valid");
        }
    }
}
