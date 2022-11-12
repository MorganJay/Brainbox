using Contracts.Repositories;
using Domain.Entities;

namespace Contracts.Common
{
    public interface IUnitOfWork
    {
        IBaseRepository<Product> Products { get; }
        IBaseRepository<Cart> Carts { get; }
        IBaseRepository<CartItem> CartItems { get; }

        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}