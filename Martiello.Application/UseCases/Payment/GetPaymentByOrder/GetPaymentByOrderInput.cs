using Martiello.Domain.Enums;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Payment.GetPaymentByOrder {
    public class GetPaymentByOrderInput : IUseCaseInput {
        public int OrderNumber { get; set; }

        public GetPaymentByOrderInput(int orderNumber) { 
            OrderNumber = orderNumber;
        }
    }
}
