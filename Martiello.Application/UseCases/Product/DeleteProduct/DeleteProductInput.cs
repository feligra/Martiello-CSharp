using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProductInput : IUseCaseInput
    {
        public DeleteProductInput(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
