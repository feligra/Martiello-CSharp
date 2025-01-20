using Microsoft.AspNetCore.Mvc;

namespace Martiello.Domain.UseCase.Interface
{
    public interface IPresenter
    {
        Task<IActionResult> Ok(IUseCaseInput input);
    }
}
