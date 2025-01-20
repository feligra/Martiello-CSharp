using AutoMapper;
using Martiello.Application.Extensions;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(CreateCustomerInput input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.Email) || input.Document == 0)
                {
                    return UseCaseOutput.Output().BadRequest("Name, email, and document are required.");
                }

                if (!input.Document.IsValidCpf())
                {
                    return UseCaseOutput.Output().BadRequest("Document number is invalid.");
                }

                Domain.Entity.Customer existingCustomer = await _customerRepository.GetCustomerByDocumentAsync(input.Document);

                if (existingCustomer != null)
                {
                    if (!existingCustomer.Active)
                    {
                        existingCustomer.Name = input.Name;
                        existingCustomer.Email = input.Email;
                        existingCustomer.Active = true;

                        await _customerRepository.UpdateCustomerAsync(existingCustomer);

                        _logger.LogInformation("Existing customer with document {Document} reactivated and updated.", input.Document);
                        return UseCaseOutput.Output(new CreateCustomerOutput()).Ok();
                    }
                    else
                    {
                        return UseCaseOutput.Output().BadRequest("Customer with the same document already exists.");
                    }
                }

                Domain.Entity.Customer customer = _mapper.Map<Domain.Entity.Customer>(input);
                customer.Active = true;

                await _customerRepository.CreateCustomerAsync(customer);

                _logger.LogInformation("Customer created successfully with document {Document}", customer.Document);

                CreateCustomerOutput output = new CreateCustomerOutput();
                return UseCaseOutput.Output(output).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating customer.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while creating the customer.");
            }
        }
    }

}
