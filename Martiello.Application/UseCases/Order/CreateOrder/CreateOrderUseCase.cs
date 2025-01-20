using AutoMapper;
using Martiello.Application.Extensions;
using Martiello.Domain.Extension;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.Interface.Service;
using Martiello.Domain.UseCase;
using Martiello.Domain.UseCase.Interface;
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

        public async Task<IUseCaseOutput> ExecuteAsync(CreateOrderInput input)
        {
            try
            {
                List<Domain.Entity.Product> products = await _productRepository.GetAllProductsAsync();

                IEnumerable<Domain.Entity.Product> orderProducts = products.Where(p => input.ProductIds.Contains(p.Id));

                if (!orderProducts.Any())
                    return UseCaseOutput.Output().BadRequest("No valid products found for the order.");

                Domain.Entity.Customer customer = new Domain.Entity.Customer();
                Domain.Entity.Order order;
                if (input.CustomerDocument.HasValue)
                {
                    if (input.CustomerDocument.Value.IsValidCpf())
                        customer = await _customerRepository.GetCustomerByDocumentAsync(input.CustomerDocument.Value);
                    else
                        return UseCaseOutput.Output().BadRequest("Document number is invalid.");

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

                CreateOrderOutput output = new CreateOrderOutput(order.Number, order.Status, qrCode);
                return UseCaseOutput.Output(output).Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating order.");
                return UseCaseOutput.Output().InternalServerError("An error occurred while creating the order.");
            }
        }
    }
}
