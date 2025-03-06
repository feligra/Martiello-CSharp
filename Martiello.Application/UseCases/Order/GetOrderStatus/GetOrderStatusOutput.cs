using Martiello.Domain.DTO;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Order.GetOrderStatus
{
    public class GetOrderStatusOutput : IUseCaseOutput
    {
        public OrderStatusDTO OrderStatus { get; set; }
        public GetOrderStatusOutput(OrderStatusDTO orderStatus)
        {
            OrderStatus = orderStatus;
        }
    }
}
