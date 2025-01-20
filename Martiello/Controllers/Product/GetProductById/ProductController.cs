using Martiello.Domain.UseCase.Interface;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using Martiello.Application.UseCases.Product.GetProductById;

namespace Martiello.Controllers.Product.GetProductById
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
        /// Obtém um produto pelo ID.
        /// </summary>
        /// <param name="id">O ID do produto a ser recuperado.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetProductByIdOutput"/> com status 200 (OK) quando o produto for encontrado.
        /// - <see cref="UseCaseOutput"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos.
        /// - <see cref="UseCaseOutput"/> com status 404 (Not Found) caso o produto não seja encontrado.
        /// - <see cref="UseCaseOutput"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(GetProductByIdOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] string id)
        {
            var input = new GetProductByIdInput(id);

            return await _presenter.Ok(input);
        }
    }
}
