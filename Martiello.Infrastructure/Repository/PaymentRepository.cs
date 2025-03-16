using Martiello.Domain.Entity;
using Martiello.Domain.Enums;
using Martiello.Domain.Interface.Repository;
using Martiello.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Martiello.Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository {
        private readonly IMongoCollection<Payment> _payment;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(DbContext context, ILogger<PaymentRepository> logger) {
            _payment = context.Payment;
            _logger = logger;
        }
        public async Task CreatePaymentAsync(Payment payment) {
            try {
                await _payment.InsertOneAsync(payment);
                _logger.LogInformation("Payment for order {OrderNumber} created", payment.OrderNumber);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while creating payment for order {OrderNumber}.", payment.OrderNumber);
                throw;
            }
        }

        public async Task<Payment> GetPaymentByOrderAsync(int orderNumber) {
            try {
                Payment? paymentResponse = await _payment.Find(p => p.OrderNumber == orderNumber).FirstOrDefaultAsync();
                if (paymentResponse == null) {
                    _logger.LogWarning("Payment with ID {orderNumber} not found.", orderNumber);
                }
                _logger.LogInformation("Payment for order {orderNumber} created", orderNumber);
                return paymentResponse;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while retrieving payment for order {orderNumber}.", orderNumber);
                throw;
            }
        }

        public async Task<List<Payment>> GetPaymentByStatusAsync(PaymentStatus status) {
            try {
                List<Payment>? paymentResponse = await _payment.Find(p => p.Status == status).ToListAsync();
                if (paymentResponse == null) {
                    _logger.LogWarning("Payment with ID {status} not found.", status);
                }
                _logger.LogInformation("Payment for order {status} created", status);
                return paymentResponse;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while retrieving payment for order {status}.", status);
                throw;
            }
        }

        public async Task UpdatePaymentStatusAsync(int orderNumber, PaymentStatus status) {
            try {
                FilterDefinition<Payment> filter = Builders<Payment>.Filter.And(
                    Builders<Payment>.Filter.Eq(o => o.OrderNumber, orderNumber),
                    Builders<Payment>.Filter.Exists("status")
                );
                UpdateDefinition<Payment> update = Builders<Payment>
                    .Update
                    .Set(o => o.Status, status)
                    .Set(o => o.UpdatedAt, DateTime.UtcNow);
                await _payment.UpdateOneAsync(filter, update);
                _logger.LogInformation("Payment for order {orderNumber} updated", orderNumber);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error while update payment for order {orderNumber}.", orderNumber);
                throw;
            }
        }
    }
}
