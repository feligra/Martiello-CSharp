using Martiello.Domain.Entity;
using Martiello.Domain.Interface.Repository;
using Martiello.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Martiello.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DbContext context, ILogger<ProductRepository> logger)
        {
            _products = context.Products;
            _logger = logger;
        }

        public async Task CreateProductAsync(Product product)
        {
            try
            {
                await _products.InsertOneAsync(product);
                _logger.LogInformation("Product with ID {Id} created successfully.", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating product with ID {Id}.", product.Id);
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                Product? product = await _products.Find(p => p.Id == id && p.Active).FirstOrDefaultAsync();
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found or inactive.", id);
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving product with ID {Id}.", id);
                throw;
            }
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            try
            {
                return await _products.Find(p => p.Category.ToLower().Contains(category.ToLower()) && p.Active).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving active products for category {Category}.", category);
                throw;
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _products.Find(p => p.Active).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all active products.");
                throw;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                product.UpdatedAt = DateTime.UtcNow;

                ReplaceOneResult result = await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    _logger.LogInformation("Product with ID {Id} updated successfully.", product.Id);
                    return true;
                }
                _logger.LogWarning("No changes were made to product with ID {Id}.", product.Id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product with ID {Id}.", product.Id);
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            try
            {
                FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
                UpdateDefinition<Product> update = Builders<Product>.Update
                    .Set(p => p.Active, false)
                    .Set(p => p.UpdatedAt, DateTime.UtcNow);

                UpdateResult result = await _products.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    _logger.LogInformation("Product with ID {Id} deleted successfully.", id);
                    return true;
                }
                _logger.LogWarning("Product with ID {Id} not found for deletion.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product with ID {Id}.", id);
                throw;
            }
        }
    }
}
