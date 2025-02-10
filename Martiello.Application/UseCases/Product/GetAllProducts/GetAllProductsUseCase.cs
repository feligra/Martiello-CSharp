
using AutoMapper;
using Martiello.Application.UseCases.Product.GetProductById;
using Martiello.Domain.DTO;
using Martiello.Domain.Entity;
using Martiello.Domain.Extension;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;
using ProductDefinition = Martiello.Domain.Entity.Product;
namespace Martiello.Application.UseCases.Product.GetAllProducts
{
    public class GetAllProductsUseCase : IUseCase<GetAllProductsInput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductsUseCase> _logger;

        public GetAllProductsUseCase(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<GetAllProductsUseCase> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IUseCaseOutput> ExecuteAsync(GetAllProductsInput input)
        {
            try
            {
                List<ProductDefinition> products = new();


                if (input.Category.HasValue)
                {
                    var category = input.Category.GetDescription();
                    products = await _productRepository.GetProductsByCategoryAsync(category);
                }
                else
                    products = await _productRepository.GetAllProductsAsync();

                if (!products.Any())
                    return UseCaseOutput.Output().NotFound($"Theres no producs.");

                var output = new GetAllProductsOutput(_mapper.Map<IEnumerable<ProductDTO>>(products));
                return UseCaseOutput.Output(output).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving products");
                return UseCaseOutput.Output().InternalServerError("An error occurred while retrieving products");
            }
        }
    }
}
