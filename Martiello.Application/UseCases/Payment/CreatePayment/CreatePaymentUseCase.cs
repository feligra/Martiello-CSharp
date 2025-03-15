using AutoMapper;
using Martiello.Application.UseCases.Customer.CreateCustomer;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martiello.Application.UseCases.Payment.CreatePayment {
    public class CreatePaymentUseCase : IUseCase<CreatePaymentInput> {
        private readonly IPaymentRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePaymentUseCase> _logger;

        public CreatePaymentUseCase(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<CreatePaymentUseCase> logger) {
            _customerRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Output> Handle(CreatePaymentInput request, CancellationToken cancellationToken) {
            try {
                OutputBuilder output = OutputBuilder.Create();

                

                _logger.LogInformation("Payment created successfully");

                return output.WithResult(new CreatePaymentOutput()).Response();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while creating Payment.");
                return OutputBuilder.Create().WithError($"An error occurred while creating the customer. {ex.Message}").BadRequestError();
            }
        }
    }
}
