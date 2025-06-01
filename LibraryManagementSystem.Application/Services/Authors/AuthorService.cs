using LibraryManagementSystem.Application.Exceptions.AuthorExceptions;
using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Application.Models.Requests;
using LibraryManagementSystem.Application.Models.Responses;
using LibraryManagementSystem.Application.Repositories;
using LibraryManagementSystem.Domain.Interfaces;
using LibraryManagementSystem.Domain.Models;
using Mapster;

namespace LibraryManagementSystem.Application.Services.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthorResponseModel> Get(int id, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.Get(id, cancellationToken).ConfigureAwait(false);
            return author.Adapt<AuthorResponseModel>();
        }
        public async Task<PagedResult<AuthorResponseModel>> GetAll(int page, int pageSize, CancellationToken cancellationToken)
        {
            var data = await _authorRepository.GetAll(page, pageSize, cancellationToken).ConfigureAwait(false);
            return data.Adapt<PagedResult<AuthorResponseModel>>();
        }
        public async Task<List<BookResponseModel>> GetBooksByAuthor(int id, CancellationToken cancellationToken)
        {
            return await _authorRepository.GetBooksByAuthor(id, cancellationToken).ConfigureAwait(false);
        }
        public async Task<AuthorResponseModel> Create(AuthorRequestModel author, CancellationToken cancellationToken)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            var entity = await _authorRepository.Get(author.Id, cancellationToken).ConfigureAwait(false);
            if (entity != null)
                throw new AuthorAlreadyExistsEx("author with such Id already exists");

            await _authorRepository.Create(author.Adapt<Author>(), cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return author.Adapt<AuthorResponseModel>();
        }
        public async Task Update(AuthorRequestModel author, CancellationToken cancellationToken)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            var entity = await _authorRepository.Get(author.Id, cancellationToken).ConfigureAwait(false);
            if (entity == null)
                throw new AuthorDoesNotExistEx("author with such Id does not exist");

            entity.Adapt(author);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _authorRepository.Get(id, cancellationToken).ConfigureAwait(false);
            if (entity == null)
                throw new AuthorDoesNotExistEx("author with such Id does not exist");

            await _authorRepository.Delete(id, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        
    }
}
