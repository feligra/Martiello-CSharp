using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martiello.Domain.UseCase.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Domain.UseCase
{
    public class Presenter : IPresenter
    {
        private readonly IServiceProvider _serviceProvider;

        public Presenter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Ok(IUseCaseInput input)
        {
            var useCaseType = typeof(IUseCase<>).MakeGenericType(input.GetType());
            var useCase = _serviceProvider.GetService(useCaseType) as dynamic;

            if (useCase == null)
            {
                return new NotFoundObjectResult($"No use case found for input type {input.GetType().Name}");
            }

            var output = await useCase.ExecuteAsync((dynamic)input) as IUseCaseOutput;

            if (output == null)
            {
                return new ObjectResult("Invalid output from use case") { StatusCode = 500 };
            }

            return output.StatusCode switch
            {
                200 => new OkObjectResult(output.Result),
                400 => new BadRequestObjectResult(output.Message),
                404 => new NotFoundObjectResult(output.Message),
                500 => new ObjectResult(output.Message) { StatusCode = 500 },
                _ => new ObjectResult("Unknown status") { StatusCode = 500 }
            };
        }
    }
}
