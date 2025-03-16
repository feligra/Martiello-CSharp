using AutoMapper;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Payment.UpdatePaymentStatus {
    public class UpdatePaymentStatusUseCase : IUseCase<UpdatePaymentStatusInput> {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePaymentStatusUseCase> _logger;

        public UpdatePaymentStatusUseCase(
            IPaymentRepository paymentRepository,
            IMapper mapper,
            ILogger<UpdatePaymentStatusUseCase> logger) {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Output> Handle(UpdatePaymentStatusInput request, CancellationToken cancellationToken) {
            try {
                OutputBuilder output = OutputBuilder.Create();

                await _paymentRepository.UpdatePaymentStatusAsync(request.OrderNumber, request.Status);

                _logger.LogInformation("Payment updated successfully");

                return output.WithResult(new UpdatePaymentStatusOutput("Payment updated successfully")).Response();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while updated Payment.");
                return OutputBuilder.Create().WithError($"An error occurred while update the customer. {ex.Message}").BadRequestError();
            }
        }
    }
}
