using Martiello.Application.UseCases.Order.CreateOrder;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Order.CreateOrder
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
        /// Cria um novo pedido com base nos produtos selecionados e informações do cliente.
        /// </summary>
        /// <param name="orderInput">Os dados necessários para a criação do pedido, incluindo IDs de produtos e informações do cliente.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="CreateOrderOutput"/> com status 201 (Created) quando o pedido for criado com sucesso.
        /// - <see cref="Output"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos ou não seja possível processar o pedido.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CreateOrderOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderInput orderInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(orderInput);
        }
    }
}
