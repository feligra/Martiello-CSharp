using AutoMapper;
using Martiello.Application.UseCases.Order.GetOrder;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Kitchen.GetOrders
{
    public class GetKitchenOrdersUseCase : IUseCase<GetKitchenOrdersInput>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetKitchenOrdersUseCase> _logger;

        public GetKitchenOrdersUseCase(IOrderRepository orderRepository, IMapper mapper, ILogger<GetKitchenOrdersUseCase> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Output> Handle(GetKitchenOrdersInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                List<Domain.Entity.Order> orders = await _orderRepository.GetAllOrdersAsync();

                if (orders == null || !orders.Any())
                    return output.WithError("No orders found.").NotFoundError();

                orders = orders.Where(o => o.Status == "Em preparação").ToList();

                List<OrderDTO> orderDTOs = orders.Select(order => _mapper.Map<OrderDTO>(order)).ToList();

                return output.WithResult(new GetOrderOutput(orderDTOs)).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving orders for kitchen.");
                return OutputBuilder.Create().WithError($"An error occurred while retrieving the orders for kitchen. {ex.Message}").InternalServerError();
            }
        }
    }
}
