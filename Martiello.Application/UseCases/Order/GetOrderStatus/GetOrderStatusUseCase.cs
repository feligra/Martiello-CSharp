using AutoMapper;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Order.GetOrderStatus
{
    public class GetOrderStatusUseCase : IUseCase<GetOrderStatusInput>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrderStatusUseCase> _logger;

        public GetOrderStatusUseCase(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrderStatusUseCase> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IUseCaseOutput> ExecuteAsync(GetOrderStatusInput input)
        {
            try
            {
                Domain.Entity.Order orderStatus = await _orderRepository.GetOrderByDocumentAsync(input.Document);

                if (orderStatus == null)
                    return UseCaseOutput.Output().NotFound($"Order status not found for Document: {input.Document}.");

                OrderStatusDTO orderStatusDTO = _mapper.Map<OrderStatusDTO>(orderStatus);
                return UseCaseOutput.Output(orderStatusDTO).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving order status.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while retrieving the order status.");
            }
        }
    }
}
