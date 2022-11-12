using System.Linq.Expressions;

namespace Contracts.Repositories
{
    public interface IBaseRepository<T>
    {
        T Add(T entity);

        T Update(T entity);

        void Remove(T entity);

        void RemoveRange(T[] entities);

        Task<T> FindByIdAsync(params object[] ids);

        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}