using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Services;
using Xunit;

namespace ProvaPub.Tests
{
    public class CustomerServiceTests
    {
        private static CustomerService GetCustomerService()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new TestDbContext(options))
            {
                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(TestDbContext.getCustomerSeed());
                }
                context.SaveChanges();
            }

            var dbContext = new TestDbContext(options);

            return new CustomerService(dbContext);
        }

        [Fact]
        public async Task CanPurchase_WithInvalidCustomerId_ShouldThrowException()
        {
            // Arrange
            var customerService = GetCustomerService();
            var nonExistentCustomerId = 0;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await customerService.CanPurchase(nonExistentCustomerId, 50)
            );
        }

        [Fact]
        public async Task CanPurchase_WithInvalidPurchaseValue_ShouldThrowException()
        {
            // Arrange
            var customerService = GetCustomerService();
            var nonExistentCustomerId = 1;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await customerService.CanPurchase(nonExistentCustomerId, 0)
            );
        }

        [Fact]
        public async Task CanPurchase_WithNonExistentCustomer_ShouldThrowException()
        {
            // Arrange
            var customerService = GetCustomerService();
            var nonExistentCustomerId = 999;

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await customerService.CanPurchase(nonExistentCustomerId, 50)
            );
        }

        [Fact]
        public async Task CanPurchase_WithNegativePurchaseValue_ShouldThrowException()
        {
            // Arrange
            var customerService = GetCustomerService();
            var customerId = 1;
            var negativePurchaseValue = -50;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await customerService.CanPurchase(customerId, negativePurchaseValue)
            );
        }

        [Fact]
        public async Task CanPurchase_WithFirstTimeCustomerAndExceedingMaxValue_ShouldReturnFalse()
        {
            // Arrange
            var customerService = GetCustomerService();
            var firstTimeCustomerId = 2;
            var exceedingMaxValue = 150;

            // Act
            var result = await customerService.CanPurchase(firstTimeCustomerId, exceedingMaxValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CanPurchase_WithValidFirstTimeCustomerPurchase_ShouldReturnTrue()
        {
            // Arrange
            var customerService = GetCustomerService();
            var firstTimeCustomerId = 2;
            var validPurchaseValue = 50;

            // Act
            var result = await customerService.CanPurchase(firstTimeCustomerId, validPurchaseValue);

            // Assert
            Assert.True(result);
        }

    }
}