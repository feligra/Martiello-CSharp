using MediatR;

namespace Martiello.Domain.UseCase
{
    public interface IUseCase<TInput> : IRequestHandler<TInput, Output>
        where TInput : IUseCaseInput
    {
    }
}
