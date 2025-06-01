using LibraryManagementSystem.Application.Exceptions.PatronExceptions;
using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;
using Mapster;

namespace LibraryManagementSystem.Application.Services.Patrons
{
    public class PatronService : IPatronService
    {
        private readonly IPatronRepository _patronRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PatronService(IPatronRepository patronRepository, IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _patronRepository = patronRepository;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PatronResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            var patron = await _patronRepository.Get(id, cancellationToken).ConfigureAwait(false);
            return patron.Adapt<PatronResponseModel>();
        }
        public async Task<PatronResponseModel> Create(PatronRequestModel patron, CancellationToken cancellationToken)
        {
            if (patron == null)
                throw new ArgumentNullException($"{nameof(patron)}");

            var entity = await _patronRepository.Get(patron.Id, cancellationToken).ConfigureAwait(false);
            if (entity == null)
                throw new PatronAlreadyExistsEx("Patron with such id already exists");

            patron.MembershipDate = DateTime.UtcNow;
            await _patronRepository.Create(patron.Adapt<Patron>(), cancellationToken).ConfigureAwait(false);
            
            return patron.Adapt<PatronResponseModel>();
        }
        public async Task Update(PatronRequestModel patron, CancellationToken cancellationToken)
        {
            if(patron == null) 
                throw new ArgumentNullException( nameof(patron));

            var entity = await _patronRepository.Get(patron.Id, cancellationToken).ConfigureAwait(false);
            if (entity == null)
                throw new PatronDoesNotExistEx("Patron with such Id does not exist");

            entity.Adapt(patron);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var patron = await _patronRepository.Get(id, cancellationToken).ConfigureAwait(false);
            if (patron == null)
                throw new PatronDoesNotExistEx("Patron with such Id does not exist");

            await _patronRepository.Delete(id, cancellationToken).ConfigureAwait(false);
        }
        public async Task<PagedResult<PatronResponseModel>> GetAll(int number, int size, CancellationToken cancellationToken)
        {
           var patrons = await _patronRepository
                .GetAll(number, size, cancellationToken)
                .ConfigureAwait(false);

            return patrons.Adapt<PagedResult<PatronResponseModel>>();
        }
        public async Task<List<BookResponseModel>> GetAllBooksbyId(int patronId, CancellationToken cancellationToken)
        {
           var books = await _bookRepository.GetAllBooksbyPatronId(patronId, cancellationToken).ConfigureAwait(false);
           return books.Adapt<List<BookResponseModel>>();
        }
    }
}
