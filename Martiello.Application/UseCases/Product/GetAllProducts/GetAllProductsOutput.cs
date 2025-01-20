using Martiello.Domain.DTO;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Product.GetAllProducts
{
    public class GetAllProductsOutput : UseCaseOutput
    {
        public IEnumerable<ProductDTO> Products { get; set; }
        public GetAllProductsOutput(IEnumerable<ProductDTO> products)
        {
            Products = products;
        }
    }
}
