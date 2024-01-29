using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Services.PaymentProcessors;

namespace ProvaPub.Services
{
	public class OrderService
	{
        private readonly Dictionary<string, IPaymentProcessor> _paymentProcessors;

        public OrderService()
        {
            _paymentProcessors = new Dictionary<string, IPaymentProcessor>
            {
                { "pix", new PixPaymentProcessor() },
                { "creditcard", new CreditCardPaymentProcessor() },
                { "paypal", new PayPalPaymentProcessor() }
            };
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
		{
            if (_paymentProcessors.TryGetValue(paymentMethod, out var paymentProcessor))
            {
                return await paymentProcessor.ProcessPayment(paymentValue, customerId);
            }

            throw new InvalidOperationException("Método de pagamento não suportado");
        }
    }
}
