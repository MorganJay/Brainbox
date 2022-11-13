using Contracts.DTOs;
using Domain.Entities;
using Domain.Models;

namespace Contracts.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> Add(CreateProductDto productDto);

        Task<PagedList<Product>> GetProducts(GetProductQuery query);

        Task<Product> Update(int productId, CreateProductDto productDto);

        Task Delete(int productId);
    }
}
