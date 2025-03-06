using Martiello.Domain.Interface.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.Services
{
    public class OrderStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderStatusBackgroundService> _logger;
        private readonly IConfiguration _configuration;

        public OrderStatusBackgroundService(IServiceProvider serviceProvider,
                                             ILogger<OrderStatusBackgroundService> logger,
                                             IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool isServiceEnabled = false;
            string isEnabledString = _configuration["OrderProcessing:UseProcessing"];

            _logger.LogInformation("OrderStatusBackgroundService is starting.");
            if (!string.IsNullOrEmpty(isEnabledString))
            {
                bool.TryParse(isEnabledString, out isServiceEnabled);
            }

            if (!isServiceEnabled)
            {
                _logger.LogInformation("OrderStatusBackgroundService is disabled in configuration. Service will not process orders.");
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }

                _logger.LogInformation("OrderStatusBackgroundService is stopping.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _serviceProvider.CreateScope();
                    IOrderRepository orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    OrderStatusUpdaterService orderStatusUpdater = scope.ServiceProvider.GetRequiredService<OrderStatusUpdaterService>();
                    List<Domain.Entity.Order> pendingOrders = await orderRepository.GetAllOrdersAsync();
                    foreach (Domain.Entity.Order order in pendingOrders)
                    {
                        if (stoppingToken.IsCancellationRequested)
                            break;
                        _logger.LogInformation("Processing order {OrderId}.", order.Id);
                        await orderStatusUpdater.UpdateOrderStatusAsync(order);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing orders.");
                }
                string checkIntervalString = _configuration["OrderProcessing:CheckIntervalInSeconds"];
                int checkInterval = string.IsNullOrEmpty(checkIntervalString) ? 30 : int.Parse(checkIntervalString);
                await Task.Delay(TimeSpan.FromSeconds(checkInterval), stoppingToken);
            }
            _logger.LogInformation("OrderStatusBackgroundService is stopping.");
        }
    }
}
