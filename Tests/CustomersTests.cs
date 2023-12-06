namespace Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using RiaApi.Controllers;
    using RiaApi.Models;
    using RiaApi.Repository;
    using Xunit;

    public class CustomerControllerTests
    {
        [Fact]
        public async Task Post_ValidCustomers_ReturnsOkResult()
        {
            // Arrange
            Mock<ICustomerRepository> mockRepository = new Mock<ICustomerRepository>();
            var controller = CreateControllerInstance(mockRepository);
            var validCustomers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", Age = 25 },
            new Customer { Id = 2, FirstName = "Alice", LastName = "Smith", Age = 30 }
        };

            // Act
            var result = await controller.Post(validCustomers);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Post_NoCustomers_ReturnsBadRequest()
        {
            // Arrange
            Mock<ICustomerRepository> mockRepository = new Mock<ICustomerRepository>();
            var controller = CreateControllerInstance(mockRepository);

            // Act
            var result = await controller.Post(Enumerable.Empty<Customer>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_CustomersWithMissingFields_ReturnsBadRequest()
        {
            // Arrange
            Mock<ICustomerRepository> mockRepository = new Mock<ICustomerRepository>();
            var controller = CreateControllerInstance(mockRepository);
            var customersWithMissingFields = new List<Customer> { new Customer() }; // Missing required fields

            // Act
            var result = await controller.Post(customersWithMissingFields);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_CustomersUnderAge_ReturnsBadRequest()
        {
            // Arrange
            Mock<ICustomerRepository> mockRepository = new Mock<ICustomerRepository>();
            var controller = CreateControllerInstance(mockRepository);
            var customersUnderAge = new List<Customer> { new Customer { Age = 18 } }; // Underage customer

            // Act
            var result = await controller.Post(customersUnderAge);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_CustomersWithExistentId_ReturnsBadRequest()
        {
            // Arrange
            Mock<ICustomerRepository> mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(r => r.HasExistentIdAsync(It.IsAny<IEnumerable<Customer>>())).ReturnsAsync(true);
            var controller = CreateControllerInstance(mockRepository);

            var customersWithExistentId = new List<Customer> { new Customer { Id = 1 } };

            // Act
            var result = await controller.Post(customersWithExistentId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private CustomerController CreateControllerInstance(Mock<ICustomerRepository> mockRepository)
        {
            // Create an instance of your controller with mock dependencies

            var mockLogger = new Mock<ILogger<CustomerController>>();
            var controller = new CustomerController(
                mockLogger.Object,
                mockRepository.Object);
            return controller;
        }
    }

}