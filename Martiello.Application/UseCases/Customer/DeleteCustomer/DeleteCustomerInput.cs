using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Customer.DeleteCustomer
{
    public class DeleteCustomerInput : IUseCaseInput
    {
        public DeleteCustomerInput(long document)
        {
            Document = document;
        }
        public long Document { get; set; }
    }
}
