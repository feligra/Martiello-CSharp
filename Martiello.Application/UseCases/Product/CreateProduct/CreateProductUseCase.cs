using AutoMapper;
using Martiello.Domain.Entity;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Product.CreateProduct
{
    public class CreateProductUseCase : IUseCase<CreateProductInput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductUseCase> _logger;

        public CreateProductUseCase(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<CreateProductUseCase> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IUseCaseOutput> ExecuteAsync(CreateProductInput input)
        {
            try
            {
                var product = _mapper.Map<Domain.Entity.Product>(input);

                await _productRepository.CreateProductAsync(product);

                _logger.LogInformation("Product created successfully with ID {Id}", product.Id);

                return UseCaseOutput.Output(product).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating product.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while creating the product.");
            }
        }
    }
}
