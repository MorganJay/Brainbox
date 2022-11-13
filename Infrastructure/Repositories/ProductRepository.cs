using Contracts.Database;
using Contracts.DTOs;
using Contracts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IMainContext context) : base(context)
        {
        }

        public async Task<Product> Add(CreateProductDto productDto)
        {
            await ValidateProductName(productDto.Name);

            var product = new Product
            {
                Category = productDto.Category,
                CreatedOn = DateTime.Now,
                Name = productDto.Name,
                NumberInStock = productDto.NumberInStock,
                Price = productDto.Price
            };

            Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task Delete(int productId)
        {
            var product = await GetProduct(productId);

            Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<Product>> GetProducts(GetProductQuery query)
        {
            if(query.Category is null && query.Name is not null)
                return await GetBy(x => x.Name.ToLower().Equals(query.Name.ToLower())).ToPagedListAsync(query.Page, query.Size);
            
            if(query.Category is not null && query.Name is null)
                return await GetBy(x => x.Category.ToLower().Equals(query.Category.ToLower())).ToPagedListAsync(query.Page, query.Size);

            return await GetAll().ToPagedListAsync(query.Page, query.Size);
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await FindByIdAsync(id);

            if (product is null) throw new CustomException("Product not found");

            return product;
        }
        
        public async Task ValidateProductName(string name)
        {
            var existingProduct = await FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));

            if (existingProduct is not null) throw new CustomException("This product name already exists");
        }

        public async Task<Product> Update(int productId, CreateProductDto productDto)
        {
            var product = await GetProduct(productId);

            var existingProduct = await FirstOrDefaultAsync(x => x.Name.ToLower().Equals(productDto.Name.ToLower()) && !x.Id.Equals(productId));

            if (existingProduct is not null) throw new CustomException("This product name already exists");

            product.Name = productDto.Name;
            product.Category = productDto.Category;
            product.NumberInStock = productDto.NumberInStock;
            product.Price = productDto.Price;

            product.LastModified = DateTime.Now;

            await _context.SaveChangesAsync();

            return product;
        }
    }
}
