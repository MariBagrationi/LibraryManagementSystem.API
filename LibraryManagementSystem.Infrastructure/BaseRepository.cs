using LibraryManagementSystem.Application.Models;
using LibraryManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class 
    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> _dbSet;
      
        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and size must be greater than zero.");

            var totalCount = await _dbSet.CountAsync(cancellationToken).ConfigureAwait(false);

            var items = await _dbSet
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new PagedResult<T>
            {
                Items = items,
                Page = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<T?> GetAsync(object[] key, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(key, cancellationToken).ConfigureAwait(false);
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false); 
        }

        public void Update(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(object[] key, CancellationToken cancellationToken)
        {
            var entity = await _dbSet.FindAsync(key, cancellationToken).ConfigureAwait(false);
            _dbSet.Remove(entity!);
        }

        public void Delete(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
        }

        public void Attach(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
        }

        public void Detach(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
