using Martiello.Domain.Enums;
using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Order.UpdateOrderStatus
{
    public class UpdateOrderStatusInput : IUseCaseInput
    {
        public long OrderNumber { get; set; }
        public OrderStatus NewStatus { get; set; }
    }
}
