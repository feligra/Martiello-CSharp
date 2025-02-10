using Martiello.Application.UseCases.Customer.DeleteCustomer;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Controllers.Customer.DeleteCustomer
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
        /// Deleta um cliente.
        /// </summary>
        /// <param name="customerDeleteInput">Os dados necessários para a criação do cliente.</param>
        /// <returns>
        /// Retorna:
        /// - <see cref="DeleteCustomerInput"/> com status 200 (Created) quando o cliente for deletado com sucesso.
        /// - <see cref="Output"/> com status 400 (Bad Request) caso os dados fornecidos sejam inválidos ou não seja possível processar a deleção do cliente.
        /// - <see cref="Output"/> com status 500 (Internal Server Error) em caso de erro interno do servidor.
        /// </returns>
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(DeleteCustomerOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Output), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomerAsync([FromBody] DeleteCustomerInput customerDeleteInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return await _presenter.OK(customerDeleteInput);
        }
    }
}
