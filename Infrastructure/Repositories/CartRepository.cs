using Contracts.Database;
using Contracts.DTOs;
using Contracts.Repositories;

using Domain.Entities;
using Domain.Exceptions;

namespace Infrastructure.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(IMainContext context) : base(context)
        {
        }

        public async Task<Cart> AddProductToCart(CreateCartDto cartDTO)
        {
            var existingCart = await GetUserCart(cartDTO.UserId);

            if (existingCart is not null) return await AddProductToExistingCart(cartDTO, existingCart);

            var newCartItems = new List<CartItem>(cartDTO.Products.Count);

            foreach (var cartItem in cartDTO.Products)
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == cartItem.ProductId);

                if (product is null) throw new CustomException($"Product id: {cartItem.ProductId} not found");

                newCartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = cartItem.Quantity                    
                });
            }

            var newCart = new Cart
            {
                CreatedOn = DateTime.Now,
                UserId = cartDTO.UserId
            };

            Add(newCart);

            await _context.SaveChangesAsync();

            newCartItems.ForEach(x => x.CartId = newCart.Id);

            _context.CartItems.AddRange(newCartItems);

            await _context.SaveChangesAsync();

            newCart.Products = newCartItems;

            return newCart;
        }

        public async Task Clear(Guid userId)
        {
            var cart = await GetUserCart(userId);

            if (cart == null) throw new CustomException("No cart found for this user");

            _context.CartItems.RemoveRange(cart.Products);

            Remove(cart);

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetUserCart(Guid userId)
        {
            return await FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Cart> RemoveProduct(Guid userId, int productId)
        {
            var cart = await GetUserCart(userId);

            if (cart == null) throw new CustomException("No cart found for this user");

            var productToRemove = cart.Products.FirstOrDefault(x => x.ProductId == productId);

            if (productToRemove == null) throw new CustomException($"product with id {productId} is not in this cart");

            cart.Products.Remove(productToRemove);

            _context.CartItems.Remove(productToRemove);

            await _context.SaveChangesAsync();

            return cart;
        }

        private async Task<Cart> AddProductToExistingCart(CreateCartDto cartDto, Cart cart)
        {
            foreach (var item in cartDto.Products)
            {
                if (cart.Products.Any(x => x.ProductId == item.ProductId))
                {
                    var cartItem = _context.CartItems.FirstOrDefault(x => x.ProductId == item.ProductId);

                    cartItem.Quantity += item.Quantity;
                }
                else
                {
                    var product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);

                    if (product is null) throw new CustomException($"Product id: {item.ProductId} not found");

                    var newProduct = new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        Quantity = item.Quantity,
                        CartId = cart.Id
                    };

                    cart.Products.Add(newProduct);

                    _context.CartItems.Add(newProduct);
                }
            }

            cart.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> UpdateProductCart(List<CreateCartItemDto> items, Guid userId)
        {
            var cart = await GetUserCart(userId);

            if (cart == null) throw new CustomException("user cart not found");

            foreach (var item in items)
            {
                var productToUpdate = cart.Products.FirstOrDefault(x => x.ProductId == item.ProductId);

                if (productToUpdate is null) throw new CustomException($"product with the id {item.ProductId} does not exist in the cart");

                if (productToUpdate.Quantity == item.Quantity) return cart;

                if (item.Quantity == 0) cart.Products.Remove(productToUpdate);
                else productToUpdate.Quantity = item.Quantity;
                
            }
            
            cart.LastModified = DateTime.Now;
            await _context.SaveChangesAsync();

            return cart;
        }
    }
}
