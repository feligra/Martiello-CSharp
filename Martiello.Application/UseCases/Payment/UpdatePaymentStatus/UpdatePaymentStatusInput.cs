using Martiello.Domain.Entity;
using Martiello.Domain.Enums;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Payment.UpdatePaymentStatus {
    public class UpdatePaymentStatusInput : IUseCaseInput {
        public int OrderNumber { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
