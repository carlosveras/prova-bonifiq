using Microsoft.EntityFrameworkCore;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService : ICustomerService
    {
        TestDbContext _ctx;

        public CustomerService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public CustomerList ListCustomers(int page)
        {
            int pageSize = 10; // Número de itens por página

            // Lógica de paginação no banco de dados
            var customers = _ctx.Customers
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalCount = _ctx.Customers.Count();

            return new CustomerList
            {
                Items = customers,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }



        #region original
        //public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        //{
        //    if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

        //    if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

        //    //Business Rule: Non registered Customers cannot purchase
        //    var customer = await _ctx.Customers.FindAsync(customerId);
        //    if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

        //    //Business Rule: A customer can purchase only a single time per month
        //    var baseDate = DateTime.UtcNow.AddMonths(-1);
        //    var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
        //    if (ordersInThisMonth > 0)
        //        return false;

        //    //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
        //    var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
        //    if (haveBoughtBefore == 0 && purchaseValue > 100)
        //        return false;

        //    return true;
        //}
        #endregion

        #region refatorada
        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            ValidateInput(customerId, purchaseValue);

            var customer = await GetCustomerAsync(customerId);

            CheckCustomerExists(customerId, customer);

            if (!await CanPurchaseOncePerMonth(customerId))
                return false;

            if (!await CanMakeFirstPurchase(customerId, purchaseValue))
                return false;

            return true;
        }

        private void ValidateInput(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(purchaseValue));
        }

        private async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _ctx.Customers.FindAsync(customerId);
        }

        private void CheckCustomerExists(int customerId, Customer customer)
        {
            if (customer == null)
                throw new InvalidOperationException($"Customer Id {customerId} does not exist");
        }

        private async Task<bool> CanPurchaseOncePerMonth(int customerId)
        {
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            return ordersInThisMonth == 0;
        }

        private async Task<bool> CanMakeFirstPurchase(int customerId, decimal purchaseValue)
        {
            var haveBoughtBefore = await _ctx.Customers.AnyAsync(s => s.Id == customerId && s.Orders.Any());
            return haveBoughtBefore || purchaseValue <= 100;
        }
        #endregion

    }
}
