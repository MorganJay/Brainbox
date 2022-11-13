using AutoMapper;
using Contracts.DTOs;
using Contracts.Repositories;
using Domain;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductsController(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<ProductDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto request)
        {
            try
            {
                if (request.Price <= 0) throw new CustomException("Price of product should be higher than zero");

                if (request.NumberInStock <= 0 && request.Stockable) throw new CustomException("Total stock of product should be higher than zero");

                var product = await _productRepository.Add(request);

                var result = _mapper.Map<ProductDto>(product);

                return ApiCreated(nameof(GetProduct), new { result.Id }, result);
            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }

        /// <summary>
        /// Get all products with pagination and search params
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PagedList<ProductDto>>), 200)]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetProductQuery query)
        {
            var pagedList = await _productRepository.GetProducts(query);

            var result = new
            {
                currentPage = pagedList.CurrentPage,
                items = _mapper.Map<List<ProductDto>>(pagedList.Items),
                pageSize = pagedList.PageSize,
                totalCount = pagedList.TotalCount,
                totalPages = pagedList.TotalPages
            };

            return ApiOk(result);
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Result<ProductDto>), 200)]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var product = await _productRepository.FindByIdAsync(id);

            if (product is null) return ApiBad(message: "Product not found");

            return ApiOk(_mapper.Map<ProductDto>(product));
        }

        /// <summary>
        /// Update details of a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] CreateProductDto request)
        {
            try
            {
                if (request.Price <= 0) throw new CustomException("Price of product should be higher than zero");

                if (request.NumberInStock <= 0 && request.Stockable) throw new CustomException("Total stock of product should be higher than zero");

                var product = await _productRepository.Update(id, request);

                var result = _mapper.Map<ProductDto>(product);

                return ApiOk(result);
            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            try
            {
                await _productRepository.Delete(id);

                return ApiOk(true);
            }
            catch (CustomException ex)
            {
                return ApiBad(message: ex.Message);
            }
        }
    }
}
