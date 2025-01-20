using Martiello.Application.UseCases.Customer.CreateCustomer;
using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Order.CreateOrder
{
    public class CreateOrderInput : IUseCaseInput
    {
        public long? CustomerDocument { get; set; }
        public List<string> ProductIds { get; set; }
    }


}
