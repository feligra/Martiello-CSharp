using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Customer.DeleteCustomer
{
    public class DeleteCustomerOutput : IUseCaseOutput
    {
        public DeleteCustomerOutput(string message, bool success)
        {
            Message = message;
            Success = success;
        }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
