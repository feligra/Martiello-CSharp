using AutoMapper;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Product.GetProductById
{
    public class GetProductByIdUseCase : IUseCase<GetProductByIdInput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductByIdUseCase> _logger;

        public GetProductByIdUseCase(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<GetProductByIdUseCase> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Output> Handle(GetProductByIdInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();
                Domain.Entity.Product product = await _productRepository.GetProductByIdAsync(request.Id);

                if (product == null)
                    return output.WithError("Product not found.").NotFoundError();

                return output.WithResult(new GetProductByIdOutput(_mapper.Map<ProductDTO>(product))).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving product with ID {Id}.", request.Id);
                return OutputBuilder.Create().WithError("An error occurred while retrieving product the product.").InternalServerError();
            }
        }

    }
}
