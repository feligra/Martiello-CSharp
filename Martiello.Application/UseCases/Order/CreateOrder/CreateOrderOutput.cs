using Martiello.Domain.DTO;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Order.CreateOrder
{
    public class CreateOrderOutput : IUseCaseOutput
    {
        public long OrderNumber { get; private set; }
        public string Status { get; private set; }
        public List<ProductOrderDTO> Products { get; private set; }
        public decimal Total { get; private set; }
        public string QrCode { get; private set; }

        public CreateOrderOutput(long orderNumber, string status, List<ProductOrderDTO> products, string qrCode)
        {
            OrderNumber = orderNumber;
            Status = status;
            Products = products;
            Total = products.Sum(p => p.Price);
            QrCode = qrCode;

        }
    }
}
