using AutoMapper;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(GetOrderInput input)
        {
            try
            {
                if (input.OrderNumber == null && input.Document == null)
                {
                    return UseCaseOutput.Output().BadRequest("Either OrderId or Document must be provided.");
                }

                List<Domain.Entity.Order> orders = null;

                if (input.OrderNumber.HasValue)
                {
                    Domain.Entity.Order order = await _orderRepository.GetOrderByNumberAsync(input.OrderNumber.Value);
                    if (order != null)
                    {
                        orders = new List<Domain.Entity.Order> { order };
                    }
                }
                else if (input.Document != null)
                {
                    orders = await _orderRepository.GetOrdersByDocumentAsync(input.Document.Value);
                }

                if (orders == null || !orders.Any())
                {
                    return UseCaseOutput.Output().NotFound("No orders found.");
                }

                List<OrderDTO> orderDTOs = orders.Select(order => _mapper.Map<OrderDTO>(order)).ToList();

                GetOrderOutput output = new GetOrderOutput(orderDTOs);
                return UseCaseOutput.Output(output).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while retrieving the orders.");
            }
        }
    }
}
