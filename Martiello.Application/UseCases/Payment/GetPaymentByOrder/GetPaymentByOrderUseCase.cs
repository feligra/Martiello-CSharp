using AutoMapper;
using Martiello.Application.UseCases.Payment.GetPaymentByOrder;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Payment.CreatePayment {
    public class GetPaymentByOrderUseCase : IUseCase<GetPaymentByOrderInput> {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPaymentByOrderUseCase> _logger;

        public GetPaymentByOrderUseCase(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<GetPaymentByOrderUseCase> logger) {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Output> Handle(GetPaymentByOrderInput request, CancellationToken cancellationToken) {
            try {
                OutputBuilder output = OutputBuilder.Create();

                Domain.Entity.Payment payment = await _paymentRepository.GetPaymentByOrderAsync(request.OrderNumber);
                if (payment == null) {
                    return output.WithError("No Payment found.").NotFoundError();
                }
                _logger.LogInformation("Payment updated successfully");

                return output.WithResult(new GetPaymentByOrderOutput(payment)).Response();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while updated Payment.");
                return OutputBuilder.Create().WithError($"An error occurred while update the customer. {ex.Message}").BadRequestError();
            }
        }
    }
}
