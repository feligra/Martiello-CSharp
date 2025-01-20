using Martiello.Domain.UseCase.Interface;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using Martiello.Application.UseCases.Product.CreateProduct;

namespace Martiello.Controllers.Product.CreateProduct
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
        /// Cria um novo produto.
        /// </summary>
        /// <param name="productInput">Os dados necessários para a criação do produto.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="CreateProductOutput"/> com status 201 (Created) quando o produto for criado com sucesso.
        /// - <see cref="UseCaseOutput"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos.
        /// - <see cref="UseCaseOutput"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CreateProductOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductInput productInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.Ok(productInput);
        }
    }
}
