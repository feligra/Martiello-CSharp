using Martiello.Domain.DTO;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Payment.GetPaymentByStatus {
    public class GetPaymentByStatusOutput : IUseCaseOutput {
        public List<Domain.Entity.Payment> Payments { get; set; }

        public GetPaymentByStatusOutput(List<Domain.Entity.Payment> payments) {
            Payments = payments;
        }
    }
}
