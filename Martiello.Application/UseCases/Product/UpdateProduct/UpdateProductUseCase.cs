using AutoMapper;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Product.UpdateProduct
{
    public class UpdateProductUseCase : IUseCase<UpdateProductInput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductUseCase> _logger;

        public UpdateProductUseCase(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<UpdateProductUseCase> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Output> Handle(UpdateProductInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                Domain.Entity.Product existingProduct = await _productRepository.GetProductByIdAsync(request.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product not found with ID {Id}", request.Id);
                    return output.WithError($"Product with ID {request.Id} not found.").NotFoundError();
                }

                _mapper.Map(request, existingProduct);
                await _productRepository.UpdateProductAsync(existingProduct);
                _logger.LogInformation("Product updated successfully with ID {Id}", existingProduct.Id);
                return output.WithResult(new UpdateProductOutput(existingProduct)).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product with ID {Id}.", request.Id);
                return OutputBuilder.Create().WithError("An error occurred while updating the product.").InternalServerError();
            }
        }
    }
}
