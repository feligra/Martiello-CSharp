using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Martiello.Domain.UseCase
{
    public class ServiceUnavailableObjectResult : ObjectResult
    {
        public ServiceUnavailableObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status503ServiceUnavailable;
        }
    }
}
