using Martiello.Domain.Enums;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Payment.GetPaymentByStatus {
    public class GetPaymentByStatusInput : IUseCaseInput {
        public PaymentStatus Status { get; set; }

        public GetPaymentByStatusInput(PaymentStatus status) { 
            Status = status;
        }
    }
}
