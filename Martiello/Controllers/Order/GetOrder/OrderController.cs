using Martiello.Application.UseCases.Order.GetOrder;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Order.GetOrder
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IPresenter _presenter;

        public OrderController(IPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// Recupera os detalhes de um pedido com base no número do pedido.
        /// </summary>
        /// <param name="input">Os dados necessários para identificar o pedido, incluindo o número do pedido.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetOrderOutput"/> com status 200 (OK) quando o/os pedido/os é/sâo encontrado/dos.
        /// - <see cref="Output"/> com status 404 (Not Found) caso o pedido não seja encontrado.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetOrderOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderAsync([FromQuery] GetOrderInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(input);
        }
    }
}
