using Microsoft.AspNetCore.Mvc;

namespace Martiello.Domain.UseCase
{
    public interface IPresenter
    {
        Task<IActionResult> OK(IUseCaseInput input);
        Task<IActionResult> NoContent(IUseCaseInput input);
        Task<IActionResult> Created(IUseCaseInput input);
        Task<IActionResult> Accepted(IUseCaseInput input);
        Task<IActionResult> Custom(IUseCaseInput input, int statusCode);
    }
}
