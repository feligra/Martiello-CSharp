using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;


namespace Martiello.Application.UseCases.Order.UpdateOrderStatus
{
    public class UpdateOrderStatusUseCase : IUseCase<UpdateOrderStatusInput>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderStatusUseCase> _logger;

        public UpdateOrderStatusUseCase(IOrderRepository orderRepository, ILogger<UpdateOrderStatusUseCase> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Output> Handle(UpdateOrderStatusInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();
                Domain.Entity.Order order = await _orderRepository.GetOrderByNumberAsync(request.OrderNumber);

                if (order == null)
                    return output.WithError($"Order with Id {request.OrderNumber} not found.").NotFoundError();

                bool success = await _orderRepository.UpdateOrderStatusAsync(request.OrderNumber, request.NewStatus);

                _logger.LogInformation("Order status updated successfully. Id: {OrderId}, NewStatus: {NewStatus}",
                    request.OrderNumber, request.NewStatus);

                

                return output.WithResult(new UpdateOrderStatusOutput()).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating order status.");
                return OutputBuilder.Create().WithError($"An error occurred while updating the order status. {ex.Message}").InternalServerError();
            }
        }
    }
}
