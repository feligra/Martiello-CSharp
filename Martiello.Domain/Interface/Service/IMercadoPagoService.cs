namespace Martiello.Domain.Interface.Service
{
    public interface IMercadoPagoService
    {
        Task<string> CreatePaymentAsync(decimal amount, string description);
    }
}
