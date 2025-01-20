using Martiello.Application.Extensions;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(DeleteCustomerInput input)
        {
            try
            {
                if (!input.Document.ToString().IsValidCpf())
                {
                    return UseCaseOutput.Output(new DeleteCustomerOutput($"Invalid document.", false)).BadRequest("Invalid document.");
                }

                Domain.Entity.Customer customer = await _customerRepository.GetCustomerByDocumentAsync(input.Document);
                if (customer == null)
                {
                    _logger.LogWarning("Customer with Document {Document} not found.", input.Document);
                    return UseCaseOutput.Output(new DeleteCustomerOutput($"Customer with Document {input.Document} not found.", false)).Ok();
                }

                bool success = await _customerRepository.DeleteCustomerAsync(customer.Id);
                if (success)
                {
                    _logger.LogInformation("Customer with ID {CustomerId} successfully deleted.", customer.Id);
                    return UseCaseOutput.Output(new DeleteCustomerOutput("Customer with ID {customer.Id} was deleted.", true)).Ok();
                }

                _logger.LogWarning("Failed to delete customer with ID {CustomerId}.", customer.Id);
                return UseCaseOutput.Output(new DeleteCustomerOutput("Failed to delete customer.", false)).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting customer.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while deleting the customer.");
            }
        }
    }
}
