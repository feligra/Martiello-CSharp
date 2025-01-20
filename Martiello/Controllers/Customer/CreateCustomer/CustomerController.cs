using Martiello.Application.UseCases.Order.CreateOrder;
using Martiello.Domain.UseCase.Interface;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using Martiello.Application.UseCases.Customer.CreateCustomer;

namespace Martiello.Controllers.Customer.CreateCustomer
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly IPresenter _presenter;

        public CustomerController(IPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="customerInput">Os dados necessários para a criação do cliente.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="CreateCustomerOutput"/> com status 201 (Created) quando o cliente for criado com sucesso.
        /// - <see cref="UseCaseOutput"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos ou não seja possível processar a criação do cliente.
        /// - <see cref="UseCaseOutput"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CreateOrderOutput), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UseCaseOutput), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerInput customerInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.Ok(customerInput);
        }
    }
}
