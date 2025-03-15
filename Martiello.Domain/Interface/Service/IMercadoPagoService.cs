using Martiello.Domain.Entity;

namespace Martiello.Domain.Interface.Service
{
    public interface IMercadoPagoService
    {
        Task<string> CreatePaymentAsync(Order order, string description);
    }
}
