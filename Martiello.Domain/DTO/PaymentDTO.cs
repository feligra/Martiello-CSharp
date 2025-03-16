

using Martiello.Domain.Enums;

namespace Martiello.Domain.DTO
{
    public class PaymentDTO
    {
        public int OrderNumber { get; set; }
        public PaymentStatus Status { get; set; }
        public string CustomerId { get; set; }
        public string Qrcode { get; set; }
    }
}
