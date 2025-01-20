using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Product.GetProductById
{
    public class GetProductByIdInput : IUseCaseInput
    {
        public string Id { get; set; }
        public GetProductByIdInput(string id)
        {
            Id = id;
        }
    }
}
