namespace Martiello.Domain.UseCase
{
    public sealed class OutputBuilder
    {
        private readonly Output _output;

        private OutputBuilder()
        {
            _output = new Output();
        }

        public OutputBuilder WithError(string message, string code = null)
        {
            Error error = new Error(message, code);
            _output.AddError(error);
            return this;
        }
        public OutputBuilder WithError(string message, params string[] parameters)
        {
            Error error = new Error(message, parameters);
            _output.AddError(error);
            return this;
        }
        public OutputBuilder WithResult<T>(T result) where T : IUseCaseOutput
        {
            _output.AddResult(result);
            return this;
        }

        public Output Response()
        {
            if (_output.Result is null)
            {

            }

            _output.ClearErrors();
            _output.ClearErrorCode();
            return _output;
        }
        public Output CustomerError(ErrorCode errorCode)
        {
            _output.SetErrorCode(errorCode);
            return _output;
        }

        public Output NotFoundError()
        {
            _output.SetErrorCode(ErrorCode.NotFound);
            return _output;
        }
        public Output BadRequestError()
        {
            _output.SetErrorCode(ErrorCode.BadRequest);
            return _output;
        }
        public Output BusinessError()
        {
            _output.SetErrorCode(ErrorCode.Business);
            return _output;
        }
        public Output UnathorizedError()
        {
            _output.SetErrorCode(ErrorCode.Unauthorized);
            return _output;
        }
        public Output InternalServerError()
        {
            _output.SetErrorCode(ErrorCode.InternalServerError);
            return _output;
        }

        public Output ConflictError()
        {
            _output.SetErrorCode(ErrorCode.Conflict);
            return _output;
        }

        public static OutputBuilder Create()
        {
            return new OutputBuilder();
        }

    }
}
