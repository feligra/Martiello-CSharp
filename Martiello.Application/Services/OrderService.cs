using Martiello.Domain.Enums;
using Martiello.Domain.Interface.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.Services
{
    public class OrderStatusUpdaterService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderStatusUpdaterService> _logger;
        private readonly IConfiguration _configuration;

        public OrderStatusUpdaterService(IOrderRepository orderRepository,
                                          ILogger<OrderStatusUpdaterService> logger,
                                          IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task UpdateOrderStatusAsync(Domain.Entity.Order order)
        {
            try
            {
                bool useRealTime = bool.Parse(_configuration["OrderProcessing:UseRealTimePreparation"]);
                double timeMultiplier = useRealTime ? 1 : 1.0 / 3.0;

                // Atualizar para "Recebido" (1m-3m)
                TimeSpan receivedDelay = RandomDelay(1, 3, timeMultiplier);
                await Task.Delay(receivedDelay);
                OrderStatus status = OrderStatus.Received;
                await _orderRepository.UpdateOrderStatusAsync(order.Number, status);
                _logger.LogInformation("Order {OrderId} updated to Received.", order.Id);

                // Atualizar para "Em preparação" (30s-1m)
                TimeSpan preparationDelay = RandomDelay(0.5, 1, timeMultiplier);
                await Task.Delay(preparationDelay);
                status = OrderStatus.InPreparation;
                await _orderRepository.UpdateOrderStatusAsync(order.Number, status);
                _logger.LogInformation("Order {OrderId} updated to InPreparation.", order.Id);

                // Tempo total de preparação baseado nos produtos
                int preparationTime = order.Products
                    .Where(p => p.TimeToPrepare.HasValue)
                    .Sum(p => p.TimeToPrepare.Value);

                preparationTime = (int)(preparationTime * timeMultiplier);
                if (preparationTime > 0)
                {
                    await Task.Delay(TimeSpan.FromMinutes(preparationTime));
                }

                // Atualizar para "Pronto"
                status = OrderStatus.Ready;
                await _orderRepository.UpdateOrderStatusAsync(order.Number, status);
                _logger.LogInformation("Order {OrderId} updated to Ready.", order.Id);

                // Aguardar confirmação antes de finalizar
                TimeSpan completionDelay = TimeSpan.FromMinutes(5 * timeMultiplier);
                Task completionTask = Task.Delay(completionDelay);
                Task confirmationTask = WaitForConfirmationAsync(order);

                // Finalizar ou confirmar
                Task completedTask = await Task.WhenAny(completionTask, confirmationTask);
                if (completedTask == confirmationTask)
                {
                    status = OrderStatus.Completed;
                    await _orderRepository.UpdateOrderStatusAsync(order.Number, status);
                    _logger.LogInformation("Order {OrderId} confirmed and completed.", order.Id);
                }
                else
                {
                    status = OrderStatus.Completed;
                    await _orderRepository.UpdateOrderStatusAsync(order.Number, status);
                    _logger.LogInformation("Order {OrderId} automatically finalized after timeout.", order.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating order status for OrderId {OrderId}.", order.Id);
            }
        }

        private async Task WaitForConfirmationAsync(Domain.Entity.Order order)
        {
            while (true)
            {
                if (await CheckConfirmationAsync(order.Id)) break;
                await Task.Delay(5000);
            }
        }

        private async Task<bool> CheckConfirmationAsync(string orderId)
        {
            return await Task.FromResult(false);
        }

        private TimeSpan RandomDelay(double minMinutes, double maxMinutes, double multiplier)
        {
            Random random = new Random();
            double delay = random.NextDouble() * (maxMinutes - minMinutes) + minMinutes;
            return TimeSpan.FromMinutes(delay * multiplier);
        }
    }
}
