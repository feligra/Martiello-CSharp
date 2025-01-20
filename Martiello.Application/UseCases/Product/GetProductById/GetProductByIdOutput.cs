using Martiello.Domain.DTO;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Product.GetProductById
{
    public class GetProductByIdOutput : UseCaseOutput
    {
        public ProductDTO Product { get; set; }
        public GetProductByIdOutput(ProductDTO product)
        {
            Product = product;
        }
    }
}
