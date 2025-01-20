using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martiello.Domain.Entity;

namespace Martiello.Domain.Interface.Repository
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(string id);
        Task<List<Product>> GetProductsByCategoryAsync(string category);
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
    }
}
