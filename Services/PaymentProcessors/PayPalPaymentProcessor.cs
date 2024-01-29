using ProvaPub.Interfaces;
using ProvaPub.Models;

namespace ProvaPub.Services.PaymentProcessors
{
    public class PayPalPaymentProcessor : IPaymentProcessor
    {
        public async Task<Order> ProcessPayment(decimal paymentValue, int customerId)
        {
            return await Task.FromResult(new Order { Value = paymentValue });
        }
    }
}
