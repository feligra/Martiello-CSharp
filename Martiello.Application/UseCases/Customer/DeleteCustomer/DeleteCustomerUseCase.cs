using Martiello.Application.Extensions;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Customer.DeleteCustomer
{
    public class DeleteCustomerUseCase : IUseCase<DeleteCustomerInput>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<DeleteCustomerUseCase> _logger;

        public DeleteCustomerUseCase(ICustomerRepository customerRepository, ILogger<DeleteCustomerUseCase> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }


        public async Task<Output> Handle(DeleteCustomerInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();
                if (!request.Document.ToString().IsValidCpf())
                {
                    return output.WithError("Invalid document.").BadRequestError();
                }

                Domain.Entity.Customer customer = await _customerRepository.GetCustomerByDocumentAsync(request.Document);
                if (customer == null)
                {
                    _logger.LogWarning("Customer with Document {Document} not found.", request.Document);
                    return output.WithError($"Customer with Document {request.Document} not found.").NotFoundError();
                }

                await _customerRepository.DeleteCustomerAsync(customer.Id);

                _logger.LogInformation("Customer with ID {CustomerId} successfully deleted.", customer.Id);
                return output.WithResult(new DeleteCustomerOutput("Customer with ID {customer.Id} was deleted.", true)).Response();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting customer.");
                return OutputBuilder.Create().WithError($"An error occurred while deleting the customer. {ex.Message}").InternalServerError();
            }
        }
    }
}
