using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(DeleteProductInput input)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(input.Id);

                if (product == null)
                    return UseCaseOutput.Output().NotFound($"Product with ID {input.Id} not found.");

                await _productRepository.DeleteProductAsync(input.Id);

                return UseCaseOutput.Output(new DeleteProductOutput()).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product");
                return UseCaseOutput.Output().InternalServerError("An error occurred while deleting the product.");
            }
        }
    }
}
