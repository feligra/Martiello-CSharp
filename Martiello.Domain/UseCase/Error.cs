namespace Martiello.Domain.UseCase
{
    public sealed class Error
    {
        public Error(string message)
        {
            Message = message;
        }
        public Error(string message, string code)
        {
            Message = message;
            Code = code;
        }
        public Error(string message, params string[] parameters)
        {
            Message = message;
            Parameters = parameters;
        }
        public Error(string message, string code, params string[] parameters)
        {
            Message = message;
            Code = code;
            Parameters = parameters;
        }
        public string Message { get; set; }
        public string Code { get; set; }
        public string[] Parameters { get; set; }
    }
}
