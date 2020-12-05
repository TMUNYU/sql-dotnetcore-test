using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Orders.Api.Controllers;
using Orders.Api.Models.Dtos.Request;
using Orders.Api.Models.Response;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System.Threading.Tasks;

namespace Orders.Api.Tests.Orders.Api.Tests
{
    [TestFixture]
    public class OrdersDetailsControllerTests
    {
        private Mock<IOrderService> _orderService;
        private Mock<ICustomerDetailsService> _customerDetailsService;

        private OrdersDetailsController _objectUnderTest;

        [SetUp]
        public void Setup()
        {
            _customerDetailsService = new Mock<ICustomerDetailsService>();
            _orderService = new Mock<IOrderService>();

            _objectUnderTest = new OrdersDetailsController(_orderService.Object, _customerDetailsService.Object, Mock.Of<ILogger<OrdersDetailsController>>());
        }

        [Test]
        public async Task GivenIdentityIsNull_WhenGetLatestOrderDetailsIsCalled_ThenBadRequestShouldReturnAsync()
        {
            // Act
            var result = await _objectUnderTest.GetLatestOrderDetails(null);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task GivenIdentityIsInvalid_WhenGetLatestOrderDetailsIsCalled_ThenBadRequestShouldReturnAsync()
        {
            // Arrange
            _objectUnderTest.ModelState.AddModelError("Email", "isInvalid");

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetails(new CustomerIdentity());

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task GivenModelIsValid_WhenGetLatestOrderByCustomerIdAsyncIsCalled_ThenEmailAndIdFromModelShouldBePassed()
        {
            // Arrange
            var identity = new CustomerIdentity
            {
                User = "this@email.com",
                CustomerId = "someid"
            };

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetails(identity);

            // Assert
            _customerDetailsService.Verify(x => x.GetCustomerDetailsByEmailAsync("this@email.com", "someid"));
        }

        [Test]
        public async Task GivenCustomerDetailsServiceIsCalled_WhenNullReturns_Then400ShouldReturnAsync()
        {
            // Arrange
            _customerDetailsService
                .Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((CustomerDetailsInfo)null);

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetails(new CustomerIdentity());

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task GivenUseIsFound_WhenGetOrderByEmailAsyncIsCalled_ThenBothTheIdAndEmailedFromIdentityShouldBePassed()
        {
            // Arrange
            _customerDetailsService.Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new CustomerDetailsInfo());
            var identity = new CustomerIdentity
            {
                User = "this@email.com",
                CustomerId = "someid"
            };

            // Act
            await _objectUnderTest.GetLatestOrderDetails(identity);

            // Assert
            _orderService.Verify(x => x.GetOrderByEmailAsync("this@email.com", "someid"));
        }

        [Test]
        public async Task GivenNoOrderWithGivenComboOfDetailsIsFound_WhenCustomerIsReturnFromCustomerService_ThenCustomerDetailsShouldReturnWithEmptyOrderArray()
        {
            // Arrange
            _customerDetailsService
                .Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new CustomerDetailsInfo() { 
                    FirstName = "Josh",
                    LastName = "Long"
                });

            _orderService
                .Setup(x => x.GetOrderByEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((OrderInfo)null);

            // Act
            var result = (ActionResult<LatestOrderDto>)(await _objectUnderTest.GetLatestOrderDetails(new CustomerIdentity()));

            // Assert
            Assert.Multiple(()=> {
                Assert.That(result.Value.Order, Is.Null);  
                Assert.That(result.Value.Customer.FirstName, Is.EqualTo("Josh"));  
                Assert.That(result.Value.Customer.LastName, Is.EqualTo("Long"));  
            });

        }
    }
}
