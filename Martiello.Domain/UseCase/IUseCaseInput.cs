using MediatR;

namespace Martiello.Domain.UseCase
{
    public interface IUseCaseInput : IRequest<Output>
    {
    }
}
