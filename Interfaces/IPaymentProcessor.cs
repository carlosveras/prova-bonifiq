using ProvaPub.Models;

namespace ProvaPub.Interfaces
{
    public interface IPaymentProcessor
    {
        Task<Order> ProcessPayment(decimal paymentValue, int customerId);
    }
}
