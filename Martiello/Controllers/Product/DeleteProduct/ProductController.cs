using Martiello.Application.UseCases.Product.DeleteProduct;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Product.DeleteProduct
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
        /// Deleta um produto pelo ID.
        /// </summary>
        /// <param name="id">O ID do produto a ser deletado.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="DeleteProductOutput"/> com status 200 (OK) quando o produto for deletado.
        /// - <see cref="Output"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos.
        /// - <see cref="Output"/> com status 404 (Not Found) caso o produto não seja encontrado.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(DeleteProductOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductByIdAsync([FromRoute] string id)
        {
            DeleteProductInput input = new DeleteProductInput(id);

            return await _presenter.OK(input);
        }
    }
}
