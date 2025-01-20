using AutoMapper;
using Martiello.Application.UseCases.Order.GetOrder;
using Martiello.Domain.DTO;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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
        public async Task<IUseCaseOutput> ExecuteAsync(GetProductByIdInput input)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(input.Id);

                if (product == null)
                    return UseCaseOutput.Output().NotFound($"Product not found.");

                var output = new GetProductByIdOutput(_mapper.Map<ProductDTO>(product));
                return UseCaseOutput.Output(output).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving product with ID {Id}.", input.Id);
                return UseCaseOutput.Output().InternalServerError("An error occurred while retrieving product the product.");
            }
        }
    }
}
