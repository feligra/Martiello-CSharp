using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Payment.GetPaymentByOrder {
    public class GetPaymentByOrderOutput : IUseCaseOutput {
        public Domain.Entity.Payment Payment {  get; set; }

        public GetPaymentByOrderOutput(Domain.Entity.Payment payment) {
            Payment = payment;
        }
    }
}
