using Contracts.Repositories;
using Domain.Entities;

namespace Contracts.Common
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IBaseRepository<Cart> Carts { get; }
        IBaseRepository<CartItem> CartItems { get; }
        IBaseRepository<User> Users { get; }

        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}