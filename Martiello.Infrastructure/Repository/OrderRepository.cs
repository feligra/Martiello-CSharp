using Martiello.Domain.Entity;
using Martiello.Domain.Enums;
using Martiello.Domain.Extension;
using Martiello.Domain.Interface.Repository;
using Martiello.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Martiello.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(DbContext context, ILogger<OrderRepository> logger)
        {
            _orders = context.Orders;
            _logger = logger;
        }

        public async Task CreateOrderAsync(Order order)
        {
            try
            {
                order.Number = await GenerateUniqueOrderNumberAsync();
                await _orders.InsertOneAsync(order);
                _logger.LogInformation("Order with ID {Id} created successfully.", order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order with ID {Id}.", order.Id);
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            try
            {
                Order? order = await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {Id} not found.", id);
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving order with ID {Id}.", id);
                throw;
            }
        }
        public async Task<Order> GetOrderByNumberAsync(long number)
        {
            try
            {
                FilterDefinition<Order> filter = Builders<Order>.Filter.And(
                    Builders<Order>.Filter.Eq(o => o.Number, number),
                    Builders<Order>.Filter.Exists("status")
                );

                Order? order = await _orders.Find(filter).FirstOrDefaultAsync();

                if (order == null)
                {
                    _logger.LogWarning("Order with Number {number} not found.", number);
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving order Number ID {number}.", number);
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            try
            {
                return await _orders.Find(o => o.Customer.Id == customerId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders for customer ID {CustomerId}.", customerId);
                throw;
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _orders.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all orders.");
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(long number, OrderStatus status)
        {
            try
            {
                string statusString = status.GetDescription();

                FilterDefinition<Order> filter = Builders<Order>.Filter.And(
                    Builders<Order>.Filter.Eq(o => o.Number, number),
                    Builders<Order>.Filter.Exists("status")
                );

                UpdateDefinition<Order> update = Builders<Order>
                    .Update
                    .Set(o => o.Status, statusString)
                    .Set(o => o.UpdatedAt, DateTime.UtcNow);

                UpdateResult result = await _orders.UpdateOneAsync(filter, update);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    _logger.LogInformation("Order with Number {number} status updated to {Status}.", number, statusString);
                    return true;
                }

                _logger.LogWarning("Order with Number {number} not found for status update.", number);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating status for order Number {number}.", number);
                throw;
            }
        }
        public async Task<bool> DeleteOrderAsync(string id)
        {
            try
            {
                DeleteResult result = await _orders.DeleteOneAsync(o => o.Id == id);
                if (result.IsAcknowledged && result.DeletedCount > 0)
                {
                    _logger.LogInformation("Order with ID {Id} deleted successfully.", id);
                    return true;
                }

                _logger.LogWarning("Order with ID {Id} not found for deletion.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting order with ID {Id}.", id);
                throw;
            }
        }

        private async Task<int> GenerateUniqueOrderNumberAsync()
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Empty;
            UpdateDefinition<Order> update = Builders<Order>.Update.Inc(o => o.Number, 1);

            FindOneAndUpdateOptions<Order> options = new FindOneAndUpdateOptions<Order>
            {
                ReturnDocument = ReturnDocument.After,
                Sort = Builders<Order>.Sort.Descending(o => o.Number),
                IsUpsert = true
            };

            Order result = await _orders.FindOneAndUpdateAsync(filter, update, options);

            return result.Number;
        }

        public async Task<Order> GetOrderByDocumentAsync(long document)
        {
            try
            {
                return await _orders.Find(order => order.Customer.Document == document).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving order for document: {document}");
                throw;
            }
        }

        public async Task<List<Order>> GetPendingOrdersAsync()
        {
            try
            {
                return await _orders.Find(order => order.Status == OrderStatus.Pending.GetDescription()).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving pending orders");
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersByDocumentAsync(long document)
        {
            try
            {
                return await _orders.Find(order => order.Customer.Document == document).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving orders for document: {document}");
                throw;
            }
        }




    }
}
