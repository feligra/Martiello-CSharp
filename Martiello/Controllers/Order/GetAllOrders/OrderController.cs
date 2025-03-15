using Martiello.Application.UseCases.Order.GetAllOrders;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Order.GetAllOrders
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
        /// Recupera todos os pedidos feitos.
        /// </summary>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetAllOrdersOutput"/> com status 200 (OK) quando o/os pedido/os é/sâo encontrado/dos.
        /// - <see cref="Output"/> com status 404 (Not Found) caso não tenha encontrado pedidos.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(GetAllOrdersOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrderAsync([FromQuery] bool filter = false)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(new GetAllOrdersInput(filter));
        }
    }
}
