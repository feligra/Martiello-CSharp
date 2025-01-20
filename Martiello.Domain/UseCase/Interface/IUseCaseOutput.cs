namespace Martiello.Domain.UseCase.Interface
{
    public interface IUseCaseOutput
    {
        int StatusCode { get; }
        object Result { get; }
        string Message { get; }
    }
}
