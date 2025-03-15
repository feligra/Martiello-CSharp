using Martiello.Domain.Entity;
using Martiello.Domain.Enums;

namespace Martiello.Domain.Interface.Repository {
    public interface IPaymentRepository {
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task UpdateStatusPaymentAsync(int orderNumber, PaymentStatus status);
        Task<Payment> GetPaymentAsync(Payment payment);
    }
}
