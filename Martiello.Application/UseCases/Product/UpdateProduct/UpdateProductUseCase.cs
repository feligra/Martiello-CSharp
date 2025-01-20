using AutoMapper;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(UpdateProductInput input)
        {
            try
            {
                Domain.Entity.Product existingProduct = await _productRepository.GetProductByIdAsync(input.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product not found with ID {Id}", input.Id);
                    return UseCaseOutput.Output().NotFound($"Product with ID {input.Id} not found.");
                }

                _mapper.Map(input, existingProduct);
                await _productRepository.UpdateProductAsync(existingProduct);
                _logger.LogInformation("Product updated successfully with ID {Id}", existingProduct.Id);
                return UseCaseOutput.Output(existingProduct).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product with ID {Id}.", input.Id);
                return UseCaseOutput.Output().InternalServerError("An error occurred while updating the product.");
            }
        }
    }
}
