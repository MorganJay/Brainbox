using Contracts.Database;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        protected readonly IMainContext _context;

        public BaseRepository(IMainContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public virtual T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual async Task<T> FindByIdAsync(params object[]? ids)
        {
            return await _dbSet.FindAsync(ids);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }
    }
}