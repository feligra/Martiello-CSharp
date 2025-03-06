using Martiello.Domain.DTO;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Order.GetOrder
{
    public class GetOrderOutput : IUseCaseOutput
    {
        public IEnumerable<OrderDTO> Orders { get; set; }

        public GetOrderOutput(IEnumerable<OrderDTO> orders)
        {
            Orders = orders;
        }
    }
}
