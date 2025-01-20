using Martiello.Domain.Enums;
using Martiello.Domain.UseCase.Interface;

namespace Martiello.Application.UseCases.Product.CreateProduct
{
    public class CreateProductInput : IUseCaseInput
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? TimeToPrepare { get; set; }
        public ProductCategory Category { get; set; }
        public string Description { get; set; }
    }
}
