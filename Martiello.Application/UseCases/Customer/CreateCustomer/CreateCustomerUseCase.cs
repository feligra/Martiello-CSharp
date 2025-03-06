using AutoMapper;
using Martiello.Application.Extensions;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Customer.CreateCustomer
{
    public class CreateCustomerUseCase : IUseCase<CreateCustomerInput>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCustomerUseCase> _logger;

        public CreateCustomerUseCase(
            ICustomerRepository customerRepository,
            IMapper mapper,
            ILogger<CreateCustomerUseCase> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Output> Handle(CreateCustomerInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || request.Document == 0)
                {
                    return output.WithError("Name, email, and document are required.").BadRequestError();
                }

                if (!request.Document.IsValidCpf())
                {
                    return output.WithError("Document number is invalid.").BadRequestError();

                }

                Domain.Entity.Customer existingCustomer = await _customerRepository.GetCustomerByDocumentAsync(request.Document);

                if (existingCustomer != null)
                {
                    if (!existingCustomer.Active)
                    {
                        existingCustomer.Name = request.Name;
                        existingCustomer.Email = request.Email;
                        existingCustomer.Active = true;

                        await _customerRepository.UpdateCustomerAsync(existingCustomer);

                        _logger.LogInformation("Existing customer with document {Document} reactivated and updated.", request.Document);
                        return output.WithResult(new CreateCustomerOutput()).Response();

                    }
                    else
                    {
                        return output.WithError("Customer with the same document already exists.").BadRequestError();
                    }
                }

                Domain.Entity.Customer customer = _mapper.Map<Domain.Entity.Customer>(request);
                customer.Active = true;

                await _customerRepository.CreateCustomerAsync(customer);

                _logger.LogInformation("Customer created successfully with document {Document}", customer.Document);

                return output.WithResult(new CreateCustomerOutput()).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating customer.");
                return OutputBuilder.Create().WithError($"An error occurred while creating the customer. {ex.Message}").BadRequestError();
            }
        }
    }

}
