using Martiello.Domain.Interface.Service;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;

namespace Martiello.Application.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {

        public MercadoPagoService(IConfiguration configuration)
        {
            var accessToken = configuration["MercadoPago:AccessToken"];
            MercadoPagoConfig.AccessToken = accessToken;
        }

        public async Task<string> CreatePaymentAsync(decimal amount, string description)
        {
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                     new PreferenceItemRequest
                     {
                         Title = description,
                         Quantity = 1,
                         CurrencyId = "BRL",
                         UnitPrice = amount
                     }
                }
            };

            Preference preference = await new PreferenceClient().CreateAsync(request);

            return preference.InitPoint; 
        }
    }
}