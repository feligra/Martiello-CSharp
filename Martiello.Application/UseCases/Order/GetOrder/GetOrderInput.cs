using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Order.GetOrder
{
    public class GetOrderInput : IUseCaseInput
    {
        public long? OrderNumber { get; set; }
        public long? Document { get; set; }
    }
}
