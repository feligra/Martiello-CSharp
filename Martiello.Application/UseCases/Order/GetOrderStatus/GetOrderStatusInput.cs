using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Order.GetOrderStatus
{
    public class GetOrderStatusInput : IUseCaseInput
    {
        public GetOrderStatusInput(long document)
        {
            Document = document;
        }
        public long Document { get; set; }
    }
}
