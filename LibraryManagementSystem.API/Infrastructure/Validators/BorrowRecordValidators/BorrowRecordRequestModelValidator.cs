using FluentValidation;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Models;

namespace LibraryManagementSystem.API.Infrastructure.Validators.BorrowRecordValidators
{
    public class BorrowRecordRequestModelValidator : AbstractValidator<BorrowRecordRequestModel>
    {
        public BorrowRecordRequestModelValidator()
        {
           
        }
    }
}
