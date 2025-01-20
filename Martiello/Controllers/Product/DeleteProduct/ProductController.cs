using Martiello.Domain.UseCase.Interface;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using Martiello.Application.UseCases.Product.DeleteProduct;

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
        /// - <see cref="UseCaseOutput"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos.
        /// - <see cref="UseCaseOutput"/> com status 404 (Not Found) caso o produto não seja encontrado.
        /// - <see cref="UseCaseOutput"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(DeleteProductOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProductByIdAsync([FromRoute] string id)
        {
            var input = new DeleteProductInput(id);

            return await _presenter.Ok(input);
        }
    }
}
