using Contracts.DTOs;
using Domain.Entities;

namespace Contracts.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> UpdateProductCart(List<CreateCartItemDto> items, Guid userId);

        Task<Cart> GetUserCart(Guid userId);

        Task<Cart> RemoveProduct(Guid userId, int productId);
        Task Clear(Guid userId);

        Task<Cart> AddProductToCart(CreateCartDto cartDTO);
    }
}
