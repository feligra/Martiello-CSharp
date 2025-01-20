using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martiello.Domain.UseCase.Interface
{
    public interface IUseCase<TInput>
       where TInput : IUseCaseInput
    {
        Task<IUseCaseOutput> ExecuteAsync(TInput input);
    }
}
