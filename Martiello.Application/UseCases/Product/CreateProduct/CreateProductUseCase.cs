using AutoMapper;
using Martiello.Domain.Interface.Repository;
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

        public async Task<Output> Handle(CreateProductInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                Domain.Entity.Product product = _mapper.Map<Domain.Entity.Product>(request);

                await _productRepository.CreateProductAsync(product);

                _logger.LogInformation("Product created successfully with ID {Id}", product.Id);

                return output.WithResult(new CreateProductOutput(product)).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating product.");
                return OutputBuilder.Create().WithError("An error occurred while creating the product.").InternalServerError();
            }
        }

    }
}
