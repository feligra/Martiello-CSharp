using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Order.GetAllOrders
{
    public class GetAllOrdersInput : IUseCaseInput
    {
        public GetAllOrdersInput(bool filterStatus = false) {
            FilterStatus = filterStatus;
        }

        public bool FilterStatus { get; set; }
    }
}
