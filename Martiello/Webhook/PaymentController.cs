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
        private readonly IConfiguration _configuration;

        public PaymentController(
            ILogger<PaymentController> logger,
            IOrderRepository orderRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _configuration = configuration;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> PaymentConfirmation()
        {
            _logger.LogInformation("Webhook de pagamento recebido");

            try
            {
                // 1. Ler o payload do request
                string requestBody;
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                // 2. Validar a origem da notificação (verificar se é do Mercado Pago)
                //if (!ValidarOrigemWebhook(requestBody))
                //{
                //    _logger.LogWarning("Origem do webhook não validada");
                //    return Unauthorized("Origem da notificação não autorizada");
                //}

                // 3. Processar o payload
                MercadoPagoPaymentResponse? notificacao = JsonConvert.DeserializeObject<MercadoPagoPaymentResponse>(requestBody);

                if (notificacao == null)
                {
                    _logger.LogWarning("Notificação de pagamento recebida com formato inválido");
                    return BadRequest("Formato de notificação inválido");
                }

                // 4. Atualizar o status do pedido
                //var resultado = await AtualizarStatusPedido(notificacao);

                //if (!resultado)
                //{
                //    _logger.LogWarning("Falha ao atualizar o status do pedido. ID da notificação: {NotificationId}", notificacao.Id);
                //    return StatusCode(500, "Falha ao processar a notificação de pagamento");
                //}

                //_logger.LogInformation("Notificação de pagamento processada com sucesso. ID: {NotificationId}", notificacao.Id);
                return Ok("Notificação processada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar notificação de pagamento");
                return StatusCode(500, "Erro interno ao processar notificação");
            }
        }
    }
}


