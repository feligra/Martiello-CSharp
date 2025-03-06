using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductOutput : IUseCaseOutput
    {
        public Domain.Entity.Product Product { get; set; }
        public UpdateProductOutput(Domain.Entity.Product product)
        {
            Product = product;
        }
    }
}
