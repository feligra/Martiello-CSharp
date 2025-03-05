using AutoMapper;
using Martiello.Application.Extensions;
using Martiello.Domain.DTO;
using Martiello.Domain.Extension;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.Interface.Service;
using Martiello.Domain.UseCase;
using Microsoft.Extensions.Logging;

namespace Martiello.Application.UseCases.Order.CreateOrder
{
    public class CreateOrderUseCase : IUseCase<CreateOrderInput>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMercadoPagoService _mercadoPagoService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderUseCase> _logger;

        public CreateOrderUseCase(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IMercadoPagoService mercadoPagoService,
            IMapper mapper,
            ILogger<CreateOrderUseCase> logger)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _mercadoPagoService = mercadoPagoService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Output> Handle(CreateOrderInput request, CancellationToken cancellationToken)
        {
            try
            {
                OutputBuilder output = OutputBuilder.Create();
                List<Domain.Entity.Product> products = await _productRepository.GetAllProductsAsync();

                List<Domain.Entity.Product> orderProducts = new List<Domain.Entity.Product>();

                foreach (string productId in request.ProductIds)
                {
                    Domain.Entity.Product product = products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        orderProducts.Add(product);
                    }
                }

                if (!orderProducts.Any())
                    return output.WithError("No valid products found for the order.").BadRequestError();

                Domain.Entity.Customer customer = new Domain.Entity.Customer();
                Domain.Entity.Order order;

                if (request.CustomerDocument.HasValue)
                {
                    long document = request.CustomerDocument.Value;
                    if (document.IsValidCpf())
                        customer = await _customerRepository.GetCustomerByDocumentAsync(document);
                    else
                        return output.WithError("Document number is invalid.").BadRequestError();

                    order = new Domain.Entity.Order(customer, orderProducts);
                }
                else
                {
                    order = new Domain.Entity.Order(orderProducts);
                }

                await _orderRepository.CreateOrderAsync(order);
                _logger.LogInformation("Order created successfully with ID {Id}", order.Id);

                string orderName = $"Pedido #{new Random().Next(1000000, 99999999)}";
                string paymentLink = await _mercadoPagoService.CreatePaymentAsync(order.TotalPrice, orderName);
                string qrCodeBase64 = Convert.ToBase64String(paymentLink.ToQRCode());
                string qrCode = $"data:image/png;base64,{qrCodeBase64}";

                List<ProductOrderDTO> productOrderDTO = orderProducts.Select(product => _mapper.Map<ProductOrderDTO>(product)).ToList();

                return output.WithResult(new CreateOrderOutput(order.Number, order.Status, productOrderDTO, qrCode)).Response();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order.");
                return OutputBuilder.Create().WithError($"An error occurred while creating the order. {ex.Message}").InternalServerError();
            }
        }
    }
}
