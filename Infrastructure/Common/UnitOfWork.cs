using Contracts.Common;
using Contracts.Database;
using Contracts.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMainContext _context;

        public UnitOfWork(IMainContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public IProductRepository Products => new ProductRepository(_context);
        public IBaseRepository<CartItem> CartItems => new BaseRepository<CartItem>(_context);
        public IBaseRepository<Cart> Carts => new BaseRepository<Cart>(_context);
        public IBaseRepository<User> Users => new BaseRepository<User>(_context);
    }
}