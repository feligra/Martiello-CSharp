using Martiello.Application.UseCases.Order.UpdateOrderStatus;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Order.UpdateOrderStatus
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
        /// Atualiza o status de um pedido.
        /// </summary>
        /// <param name="orderInput">Os dados necessários para a atualização do pedido.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="UpdateOrderStatusOutput"/> com status 202 (Accepted) quando o pedido for atualizado com sucesso.
        /// - <see cref="Output"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos ou não seja possível processar o pedido.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPut]
        [Route("update/status")]
        [ProducesResponseType(typeof(UpdateOrderStatusOutput), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromBody] UpdateOrderStatusInput orderInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(orderInput);
        }
    }
}
