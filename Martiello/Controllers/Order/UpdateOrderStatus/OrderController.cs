using Martiello.Application.UseCases.Order.UpdateOrderStatus;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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
        /// - <see cref="UpdateOrderStatusOutput"/> com status 201 (Created) quando o pedido for criado com sucesso.
        /// - <see cref="UseCaseOutput"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos ou não seja possível processar o pedido.
        /// - <see cref="UseCaseOutput"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPut]
        [Route("update/status")]
        [ProducesResponseType(typeof(UpdateOrderStatusOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] UpdateOrderStatusInput orderInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.Ok(orderInput);
        }
    }
}
