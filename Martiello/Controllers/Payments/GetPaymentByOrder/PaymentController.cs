using Martiello.Application.UseCases.Payment.GetPaymentByOrder;
using Martiello.Application.UseCases.Payment.GetPaymentByStatus;
using Martiello.Domain.Enums;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Paymemt.GetPaymentByOrder {
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase {
        private readonly IPresenter _presenter;

        public PaymentController(IPresenter presenter) {
            _presenter = presenter;
        }

        /// <summary>
        /// Busca os pagamentos pelo pedido.
        /// </summary>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetPaymentByOrderOutput"/> com status 200 (OK) quando o/os pagamento é/são encontrado.
        /// - <see cref="Output"/> com status 404 (Not Found) caso não tenha encontrado pagamentos.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [Route("status/{orderNumber}")]
        [ProducesResponseType(typeof(GetPaymentByOrderOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaymentByOrderAsync([FromRoute] int orderNumber) {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(new GetPaymentByOrderInput(orderNumber));
        }
    }
}
