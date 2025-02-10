using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Order.CreateOrder
{
    public class CreateOrderOutput : IUseCaseOutput
    {
        public long OrderNumber { get; private set; }
        public string Status { get; private set; }
        public string QrCode { get; private set; }

        public CreateOrderOutput(long orderNumber, string status, string qrCode)
        {
            OrderNumber = orderNumber;
            Status = status;
            QrCode = qrCode;
        }
    }
}
