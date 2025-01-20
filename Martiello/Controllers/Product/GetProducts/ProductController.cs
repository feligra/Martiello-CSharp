using Martiello.Domain.UseCase.Interface;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using Martiello.Application.UseCases.Product.GetAllProducts;
using Martiello.Domain.Enums;

namespace Martiello.Controllers.Product.GetProducts
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
        /// Obtém a lista de todos os produtos.
        /// </summary>
        /// <returns>
        /// Retorna:
        /// - <see cref="GetAllProductsOutput"/> com status 200 (OK) contendo a lista de produtos.
        /// - <see cref="UseCaseOutput"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos.
        /// - <see cref="UseCaseOutput"/> com status 404 (Not Found) caso os produtos não sejam encontrados.
        /// - <see cref="UseCaseOutput"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(GetAllProductsOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductCategory? category)
        {
            return await _presenter.Ok(new GetAllProductsInput(category));
        }
    }
}
