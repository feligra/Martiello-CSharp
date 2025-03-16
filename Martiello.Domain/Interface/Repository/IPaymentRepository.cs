using Martiello.Domain.Entity;
using Martiello.Domain.Enums;

namespace Martiello.Domain.Interface.Repository {
    public interface IPaymentRepository {
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentStatusAsync(int orderNumber, PaymentStatus status);
        Task<Payment> GetPaymentByOrderAsync(int orderNumber);
        Task<List<Payment>> GetPaymentByStatusAsync(PaymentStatus status);
    }
}
