using Microsoft.AspNetCore.Mvc;

namespace Martiello.Domain.UseCase
{
    public class CustomStatusCodeObjectResult : ObjectResult
    {
        public CustomStatusCodeObjectResult(int statusCode, object value) : base(value)
        {
            StatusCode = statusCode;
        }
    }
}
