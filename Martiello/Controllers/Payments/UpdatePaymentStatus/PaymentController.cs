using Martiello.Application.UseCases.Payment.UpdatePaymentStatus;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Paymemt.UpdatePaymentByStatus {
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase {
        private readonly IPresenter _presenter;

        public PaymentController(IPresenter presenter) {
            _presenter = presenter;
        }

        /// <summary>
        /// Atualiza o status de pagamento do pedido.
        /// </summary>
        /// <returns>
        /// Retorna:
        /// - <see cref="UpdatePaymentStatusOutput"/> com status 200 (OK) quando o pagamento é atualizado.
        /// - <see cref="Output"/> com status 404 (Not Found) caso não tenha encontrado a ordem de pagamento.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPut]
        [Route("update/status")]
        [ProducesResponseType(typeof(UpdatePaymentStatusOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePaymentStatusAsync([FromBody] UpdatePaymentStatusInput request) {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(request);
        }
    }
}
