using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Product.CreateProduct
{
    public class CreateProductOutput : IUseCaseOutput
    {
        public Domain.Entity.Product Product { get; set; }
        public CreateProductOutput(Domain.Entity.Product product)
        {
            Product = product;
        }
    }
}
