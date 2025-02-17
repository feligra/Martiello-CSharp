using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Product.DeleteProduct
{
    public class DeleteProductUseCase : IUseCase<DeleteProductInput>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteProductUseCase> _logger;

        public DeleteProductUseCase(
            IProductRepository productRepository,
            ILogger<DeleteProductUseCase> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Output> Handle(DeleteProductInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                Domain.Entity.Product product = await _productRepository.GetProductByIdAsync(request.Id);

                if (product == null)
                    return output.WithError($"Product with ID {request.Id} not found.").NotFoundError();

                await _productRepository.DeleteProductAsync(request.Id);

                return output.WithResult(new DeleteProductOutput()).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product");
                return OutputBuilder.Create().WithError("An error occurred while deleting the product.").InternalServerError();
            }
        }

    }
}
