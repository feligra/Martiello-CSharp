using AutoMapper;
using Martiello.Application.UseCases.Order.GetOrder;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Order.GetAllOrders
{
    public class GetAllOrdersUseCase : IUseCase<GetAllOrdersInput>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllOrdersUseCase> _logger;

        public GetAllOrdersUseCase(IOrderRepository orderRepository, IMapper mapper, ILogger<GetAllOrdersUseCase> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Output> Handle(GetAllOrdersInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                List<Domain.Entity.Order> orders = await _orderRepository.GetAllOrdersAsync();

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
