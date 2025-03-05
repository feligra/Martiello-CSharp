using Martiello.Domain.Entity;
using Martiello.Domain.Enums;

namespace Martiello.Domain.Interface.Repository
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> GetOrderByNumberAsync(long number);
        Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
        Task<bool> DeleteOrderAsync(string id);
        Task<Order> GetOrderByDocumentAsync(long document);
        Task<List<Order>> GetOrdersByDocumentAsync(long document);
        Task<bool> UpdateOrderStatusAsync(long number, OrderStatus status);
        Task<List<Order>> GetPendingOrdersAsync();
        Task<List<Order>> GetAllOrdersAsync();
    }
}
