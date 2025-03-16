using AutoMapper;
using Martiello.Application.UseCases.Payment.GetPaymentByStatus;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Payment.CreatePayment {
    public class GetPaymentByStatusUseCase : IUseCase<GetPaymentByStatusInput> {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPaymentByStatusUseCase> _logger;

        public GetPaymentByStatusUseCase(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<GetPaymentByStatusUseCase> logger) {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Output> Handle(GetPaymentByStatusInput request, CancellationToken cancellationToken) {
            try {
                OutputBuilder output = OutputBuilder.Create();

                List<Domain.Entity.Payment> payments = await _paymentRepository.GetPaymentByStatusAsync(request.Status);
                if (payments == null) {
                    return output.WithError("No payments founds.").NotFoundError();
                }
                _logger.LogInformation("Payment updated successfully");
                
                return output.WithResult(new GetPaymentByStatusOutput(payments)).Response();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while updated Payment.");
                return OutputBuilder.Create().WithError($"An error occurred while update the customer. {ex.Message}").BadRequestError();
            }
        }
    }
}
