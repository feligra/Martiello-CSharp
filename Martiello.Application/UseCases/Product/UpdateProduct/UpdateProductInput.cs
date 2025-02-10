using Martiello.Domain.Enums;
using Martiello.Domain.UseCase;

namespace Martiello.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductInput : IUseCaseInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? TimeToPrepare { get; set; }
        public ProductCategory Category { get; set; }
        public string Description { get; set; }
    }
}
