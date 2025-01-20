using System.Text.Json.Serialization;
using Martiello.Domain.UseCase.Interface;

namespace Martiello.Domain.UseCase
{
    public class UseCaseOutput : IUseCaseOutput
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public object Result { get; private set; }

        [JsonIgnore]
        public string Message { get; private set; }

        [JsonIgnore]
        private bool IsError => StatusCode >= 400;

        protected UseCaseOutput(object result = null)
        {
            Result = result;
        }

        public static UseCaseOutput Output(object result = null)
        {
            return new UseCaseOutput(result);
        }

        public IUseCaseOutput Ok()
        {
            StatusCode = 200;
            return this;
        }

        public IUseCaseOutput BadRequest(string message)
        {
            StatusCode = 400;
            Message = message;
            return this;
        }

        public IUseCaseOutput NotFound(string message)
        {
            StatusCode = 404;
            Message = message;
            return this;
        }

        public IUseCaseOutput InternalServerError(string message)
        {
            StatusCode = 500;
            Message = message;
            return this;
        }

        [JsonPropertyName("")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public object ResponseContent
        {
            get
            {
                if (IsError)
                {
                    // Criando um objeto anônimo que sempre incluirá statusCode e message
                    return new ErrorResponse
                    {
                        StatusCode = StatusCode,
                        Message = Message ?? string.Empty
                    };
                }

                return Result;
            }
        }
    }

    public class ErrorResponse
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
