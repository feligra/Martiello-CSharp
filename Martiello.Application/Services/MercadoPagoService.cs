using Martiello.Domain.Entity;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.Interface.Service;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace Martiello.Application.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly bool UseMercadoPago;
        private readonly IPaymentRepository _paymentRepository;
        public MercadoPagoService(IConfiguration configuration, IPaymentRepository paymentRepository)
        {
            string accessToken = configuration["MercadoPago:AccessToken"];
            UseMercadoPago = bool.Parse(configuration["MercadoPago:UseApi"]);
            MercadoPagoConfig.AccessToken = accessToken;
            _paymentRepository = paymentRepository;
        }

        public async Task<string> CreatePaymentAsync(Order order, string description)
        {
            if (UseMercadoPago)
            {

                PreferenceRequest request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                {
                     new PreferenceItemRequest
                     {
                         Title = description,
                         Quantity = 1,
                         CurrencyId = "BRL",
                         UnitPrice = order.TotalPrice
                     }
                }
                };

                Preference preference = await new PreferenceClient().CreateAsync(request);
                await _paymentRepository.CreatePaymentAsync(new Payment(
                    order.Number,
                    order.Customer,
                    Domain.Enums.PaymentStatus.Pending,
                    preference.InitPoint
                ));
                return preference.InitPoint;
            }
            else
            {
                string randomString = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 500).Select(s => s[new Random().Next(s.Length)]).ToArray());
                await _paymentRepository.CreatePaymentAsync(new Payment(
                    order.Number,
                    order.Customer,
                    Domain.Enums.PaymentStatus.Pending,
                    randomString
                ));
                return randomString;
            }
        }
    }
}