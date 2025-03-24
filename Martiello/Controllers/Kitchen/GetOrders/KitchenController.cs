using Martiello.Application.UseCases.Kitchen.GetOrders;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Kitchen.GetOrders
{
    [ApiController]
    [Route("api/kitchen")]
    public class KitchenController : ControllerBase
    {
        private readonly IPresenter _presenter;

        public KitchenController(IPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// Recupera os pedidos que são da cozinha, todos aqueles marcados como em preparação.
        /// </summary>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetKitchenOrdersOutput"/> com status 200 (OK) quando o/os pedido/os é/sâo encontrado/dos.
        /// - <see cref="Output"/> com status 404 (Not Found) caso não existam pedidos.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetKitchenOrdersOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetKitchensOrderAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(new GetKitchenOrdersInput());
        }
    }
}
