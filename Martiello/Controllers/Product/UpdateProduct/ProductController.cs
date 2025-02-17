using Martiello.Application.UseCases.Product.UpdateProduct;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Product.UpdateProduct
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IPresenter _presenter;

        public ProductController(IPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// Atualiza as informações de um produto existente.
        /// </summary>
        /// <param name="updateInput">Os dados do produto a serem atualizados.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="UpdateProductOutput"/> com status 200 (OK) quando a atualização for bem-sucedida.
        /// - <see cref="Output"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(UpdateProductOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProductAsync([FromBody] UpdateProductInput updateInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(updateInput);
        }
    }
}
