using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(UpdateOrderStatusInput input)
        {
            try
            {
                Domain.Entity.Order order = await _orderRepository.GetOrderByNumberAsync(input.OrderNumber);

                if (order == null)
                    return UseCaseOutput.Output().NotFound($"Order with Id {input.OrderNumber} not found.");

                bool success = await _orderRepository.UpdateOrderStatusAsync(input.OrderNumber, input.NewStatus);

                _logger.LogInformation("Order status updated successfully. Id: {OrderId}, NewStatus: {NewStatus}",
                    input.OrderNumber, input.NewStatus);

                return UseCaseOutput.Output().Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating order status.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while updating the order status.");
            }
        }
    }
}
