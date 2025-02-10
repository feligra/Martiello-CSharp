using System.Dynamic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Martiello.Domain.UseCase
{
    internal class Presenter : IPresenter
    {
        private readonly IMediator _mediator;
        private readonly PresenterOptions _options;

        public Presenter(IMediator mediator, IOptions<PresenterOptions> options)
        {
            _mediator = mediator;
            _options = options.Value;
        }

        public PresenterOptions Options { get { return _options; } }

        public async Task<IActionResult> Accepted(IUseCaseInput input)
        {
            Output output = await _mediator.Send(input);

            if (output.ErrorCode != null)
            {
                return CreateErrorResult(output);
            }

            return new AcceptedResult();
        }

        public async Task<IActionResult> Created(IUseCaseInput input)
        {
            Output output = await _mediator.Send(input);

            if (output.ErrorCode != null)
            {
                return CreateErrorResult(output);
            }

            return new CreatedResult(string.Empty, GetResult(output));
        }

        public async Task<IActionResult> Custom(IUseCaseInput input, int statusCode)
        {
            Output output = await _mediator.Send(input);

            if (output.ErrorCode != null)
            {
                return CreateErrorResult(output);
            }

            return new CustomStatusCodeObjectResult(statusCode, GetResult(output));
        }

        public async Task<IActionResult> NoContent(IUseCaseInput input)
        {
            Output output = await _mediator.Send(input);

            if (output.ErrorCode != null)
            {
                return CreateErrorResult(output);
            }

            return new NoContentResult();
        }

        public async Task<IActionResult> OK(IUseCaseInput input)
        {
            Output output = await _mediator.Send(input);

            if (output.ErrorCode != null)
            {
                return CreateErrorResult(output);
            }

            return new OkObjectResult(GetResult(output));
        }

        private object GetResult(Output output)
        {
            if (_options.WrapResult)
            {
                if (_options.ResultKeyDescription != "Result")
                {
                    dynamic result = new ExpandoObject();
                    IDictionary<string, object> finalResult = result;

                    finalResult.Add(_options.ResultKeyDescription, output.Result);
                    return result;
                }
                return output;
            }

            return output.Result;
        }

        private object GetErrorResult(Output output)
        {
            dynamic result = new ExpandoObject();
            IDictionary<string, object> finalResult = result;

            finalResult.Add(_options.ErrorKeyDescription, output.Errors);

            if (output.Result != null)
            {
                finalResult.Add(_options.ResultKeyDescription, output.Result);
            }

            if (output.ErrorCode.HasValue)
            {
                finalResult.Add(_options.ErrorCodeKeyDescription, output.ErrorCode.Value);
            }

            return result;
        }

        private IActionResult CreateErrorResult(Output output)
        {
            switch (output.ErrorCode)
            {
                case ErrorCode.NotFound:
                    return new NotFoundObjectResult(GetErrorResult(output));
                case ErrorCode.Business:
                    return new UnprocessableEntityObjectResult(GetErrorResult(output));
                case ErrorCode.Unauthorized:
                    return new UnauthorizedObjectResult(GetErrorResult(output));
                case ErrorCode.InternalServerError:
                    return new CustomStatusCodeObjectResult(500, GetErrorResult(output));
                case ErrorCode.Conflict:
                    return new ConflictObjectResult(GetErrorResult(output));
                case ErrorCode.ServiceUnavailable:
                    return new ServiceUnavailableObjectResult(GetErrorResult(output));
                default:
                    return new BadRequestObjectResult(GetErrorResult(output));
            }
        }


    }
}
