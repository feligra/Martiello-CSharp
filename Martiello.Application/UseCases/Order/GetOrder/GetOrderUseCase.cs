using AutoMapper;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Order.GetOrder
{
    public class GetOrderUseCase : IUseCase<GetOrderInput>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrderUseCase> _logger;

        public GetOrderUseCase(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrderUseCase> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Output> Handle(GetOrderInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();
                if (request.OrderNumber == null && request.Document == null)
                {
                    return output.WithError("Either OrderId or Document must be provided.").BadRequestError();
                }

                List<Domain.Entity.Order> orders = null;

                if (request.OrderNumber.HasValue)
                {
                    Domain.Entity.Order order = await _orderRepository.GetOrderByNumberAsync(request.OrderNumber.Value);
                    if (order != null)
                    {
                        orders = new List<Domain.Entity.Order> { order };
                    }
                }
                else if (request.Document != null)
                {
                    orders = await _orderRepository.GetOrdersByDocumentAsync(request.Document.Value);
                }

                if (orders == null || !orders.Any())
                    return output.WithError("No orders found.").NotFoundError();


                List<OrderDTO> orderDTOs = orders.Select(order => _mapper.Map<OrderDTO>(order)).ToList();

                return output.WithResult(new GetOrderOutput(orderDTOs)).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders.");
                return OutputBuilder.Create().WithError($"An error occurred while retrieving the orders. {ex.Message}").InternalServerError();
            }
        }


    }
}
