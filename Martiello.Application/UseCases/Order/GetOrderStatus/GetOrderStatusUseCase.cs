using AutoMapper;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
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

        public async Task<Output> Handle(GetOrderStatusInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();
                Domain.Entity.Order orderStatus = await _orderRepository.GetOrderByDocumentAsync(request.Document);

                if (orderStatus == null)
                    return output.WithError($"Order status not found for Document: {request.Document}.").NotFoundError();


                OrderStatusDTO orderStatusDTO = _mapper.Map<OrderStatusDTO>(orderStatus);
                return output.WithResult(new GetOrderStatusOutput(orderStatusDTO)).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving order status.");
                return OutputBuilder.Create().WithError($"An error occurred while retrieving the order status. {ex.Message}").InternalServerError();
            }
        }
    }
}
