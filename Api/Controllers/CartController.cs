using AutoMapper;
using Contracts.DTOs;
using Contracts.Managers;
using Contracts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CartController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserManager<User> _userManager;
        private readonly ICartRepository _cartRepository;

        public CartController(IMapper mapper, ICartRepository cartRepository, IUserManager<User> userManager)
        {
            _mapper = mapper;
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Create new cart for a user with products
        /// </summary>
        /// <param name="cartDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddProductsToCart([FromBody] CreateCartDto cartDto)
        {
            try
            {
                if (cartDto.Products.Any(x => cartDto.Products.Count(y => y.ProductId == x.ProductId) > 1)) return ApiBad(message: "duplicate product");
                
                if (cartDto.Products.Any(x => x.Quantity <= 0)) return ApiBad(message: "Invalid quantity for an item in products");

                var user = await _userManager.FindByIdAsync(cartDto.UserId);

                if (user is null) throw new CustomException("Only registered users can add products to cart");

                var cart = await _cartRepository.AddProductToCart(cartDto);

                return ApiOk(_mapper.Map<CartDto>(cart));
            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }

        /// <summary>
        /// Allows users view products in their cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCart([FromRoute] Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) throw new CustomException("Only registered users can view products in cart");

            var cart = await _cartRepository.GetUserCart(userId);

            if (cart is null) return ApiBad(message: "User doesn't have a cart");

            return ApiOk(_mapper.Map<CartDto>(cart));
        }

        /// <summary>
        /// Allow users remove an item from their cart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart([FromRoute] Guid userId, [FromRoute] int productId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user is null) throw new CustomException("Only registered users can remove products from cart");

                var result = await _cartRepository.RemoveProduct(userId, productId);

                return ApiOk(_mapper.Map<CartDto>(result));
            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }
        
        /// <summary>
        /// Allow users clear their cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveCart([FromRoute] Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user is null) throw new CustomException("Only registered users can remove products from cart");

                await _cartRepository.Clear(userId);

                return ApiOk(true);
            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }

        /// <summary>
        /// Allows users update quantity of a product in cart
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateCart([FromRoute] Guid userId, [FromBody] List<CreateCartItemDto> items)
        {
            try
            {
                if (items.Any(x => x.Quantity <= 0)) return ApiBad(message: "Invalid quantity for an item in products");

                var result = await _cartRepository.UpdateProductCart(items, userId);

                return ApiOk(_mapper.Map<CartDto>(result));

            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }
    }
}
