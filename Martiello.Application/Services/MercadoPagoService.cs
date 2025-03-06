using Martiello.Domain.Interface.Service;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;

namespace Martiello.Application.Services
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly bool UseMercadoPago;
        public MercadoPagoService(IConfiguration configuration)
        {
            string accessToken = configuration["MercadoPago:AccessToken"];
            UseMercadoPago = bool.Parse(configuration["MercadoPago:UseApi"]);
            MercadoPagoConfig.AccessToken = accessToken;
        }

        public async Task<string> CreatePaymentAsync(decimal amount, string description)
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
                         UnitPrice = amount
                     }
                }
                };

                Preference preference = await new PreferenceClient().CreateAsync(request);

                return preference.InitPoint;
            }
            else
            {
                string randomString = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 500).Select(s => s[new Random().Next(s.Length)]).ToArray());
                return randomString;
            }
        }
    }
}