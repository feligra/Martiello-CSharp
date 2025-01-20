using Martiello.Domain.Enums;
using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Product.GetAllProducts
{
    public class GetAllProductsInput : IUseCaseInput
    {
        public GetAllProductsInput(ProductCategory? category)
        {
            Category = category;
        }
        public ProductCategory? Category { get; set; }
    }
}
