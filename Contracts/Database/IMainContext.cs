using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Database
{
    public interface IMainContext
    {
        DbSet<Product> Products { get; }
        DbSet<Cart> Carts { get; }
        DbSet<CartItem> CartItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}