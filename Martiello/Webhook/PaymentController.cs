using System.Text;
using Martiello.Domain.Http;
using Martiello.Domain.Interface.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Martiello.Webhook
{
    [ApiController]
    [Route("webhook")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(
            ILogger<PaymentController> logger,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> PaymentConfirmation()
        {
            _logger.LogInformation("Payment notification received.");

            try
            {
                string requestBody;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
                _logger.LogInformation("Request body read successfully.");

                MercadoPagoPaymentResponse? notification = JsonConvert.DeserializeObject<MercadoPagoPaymentResponse>(requestBody);
                if (notification == null)
                {
                    _logger.LogWarning("Failed to deserialize the notification payload.");
                    return BadRequest("Invalid payload.");
                }

                if (string.IsNullOrEmpty(notification.Data?.Id))
                {
                    _logger.LogWarning("Notification data id is null or empty.");
                    return BadRequest("Missing payment id.");
                }

                if (!long.TryParse(notification.Data.Id, out long orderId))
                {
                    _logger.LogWarning($"Notification data id '{notification.Data.Id}' is not a valid number.");
                    return BadRequest("Invalid payment id format.");
                }


                bool orderExists = await _orderRepository.GetOrderByNumberAsync(orderId) != null;
                if (orderExists)
                {
                    if (notification.Action == "payment.updated")
                    {
                        await _orderRepository.UpdateOrderStatusAsync(orderId, Domain.Enums.OrderStatus.Received);
                        _logger.LogInformation($"Order {orderId} status updated to Received.");

                        await _paymentRepository.UpdatePaymentStatusAsync((int)orderId, Domain.Enums.PaymentStatus.Approved);
                        _logger.LogInformation($"Payment status set to approved for order {orderId}");
                    }
                    else
                    {
                        await _paymentRepository.UpdatePaymentStatusAsync((int)orderId, Domain.Enums.PaymentStatus.Refused);
                        _logger.LogInformation($"Payment status set to approved for order {orderId}");
                    }
                }
                else
                {
                    _logger.LogWarning($"Order with id {orderId} not found.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment notification.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}


