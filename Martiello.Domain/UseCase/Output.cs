namespace Martiello.Domain.UseCase
{
    public sealed class Output
    {
        public object Result { get; set; }
        public ErrorCode? ErrorCode { get; set; }
        public IList<Error> Errors { get; set; }

        internal void AddResult<T>(T result) where T : IUseCaseOutput
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            Result = result;
        }

        internal void AddError(Error error)
        {
            if (Errors is null)
                Errors = new List<Error>();

            Errors.Add(error);
        }

        internal void SetErrorCode(ErrorCode? errorCode)
        {
            ErrorCode = errorCode;
        }
        internal void ClearErrorCode()
        {
            ErrorCode = null;
        }

        internal void ClearErrors()
        {
            Errors = null;
        }

    }
}
