
using AutoMapper;
using Martiello.Domain.DTO;
using Martiello.Domain.Extension;
using Martiello.Domain.Interface.Repository;
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
        public async Task<Output> Handle(GetAllProductsInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();

                List<ProductDefinition> products = new();

                if (request.Category.HasValue)
                {
                    string category = request.Category.GetDescription();
                    products = await _productRepository.GetProductsByCategoryAsync(category);
                }
                else
                    products = await _productRepository.GetAllProductsAsync();

                if (!products.Any())
                    return output.WithError("Theres no producs.").Response();

                return output.WithResult(new GetAllProductsOutput(_mapper.Map<IEnumerable<ProductDTO>>(products))).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving products");
                return OutputBuilder.Create().WithError("An error occurred while retrieving products").InternalServerError();
            }
        }
    }
}
