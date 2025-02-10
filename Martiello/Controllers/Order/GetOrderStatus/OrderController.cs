using Martiello.Application.UseCases.Order.GetOrderStatus;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Order.GetOrderStatus
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
        /// Recupera o status dos pedidos em aberto.
        /// </summary>
        /// <param name="document">Documento do cliente</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetOrderStatusOutput"/> com status 200 (OK) quando os pedidos são encontrados.
        /// - <see cref="Output"/> com status 404 (Not Found) caso nenhum pedido seja encontrado.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [Route("status/{document}")]
        [ProducesResponseType(typeof(GetOrderStatusOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdersStatusAsync(long document)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(new GetOrderStatusInput(document));
        }
    }
}
