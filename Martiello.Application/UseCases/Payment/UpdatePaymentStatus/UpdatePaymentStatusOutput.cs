using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Payment.UpdatePaymentStatus {
    public class UpdatePaymentStatusOutput : IUseCaseOutput {
        public bool Success { get; set; }
        public string Message { get; set; }

        public UpdatePaymentStatusOutput(string message, bool success = true) { 
            Message = message;
            Success = success;
        }
    }
}
