using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Orders.Api.Controllers;
using Orders.Api.Models.Dtos.Request;
using Orders.Api.Services.Exceptions;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Orders.Api.Tests.Orders.Api.Tests
{
    [TestFixture]
    public class OrdersDetailsControllerTests
    {
        private Mock<ITrackingService> _trackingService;

        private OrdersDetailsController _objectUnderTest;

        [SetUp]
        public void Setup()
        {
            _trackingService = new Mock<ITrackingService>();

            _objectUnderTest = new OrdersDetailsController(_trackingService.Object, Mock.Of<ILogger<OrdersDetailsController>>());
        }

        [Test]
        public async Task GivenIdentityIsNull_WhenGetLatestOrderDetailsIsCalled_ThenBadRequestShouldReturnAsync()
        {
            // Act
            var result = await _objectUnderTest.GetLatestOrderDetailsAsync(null);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task GivenIdentityIsInvalid_WhenGetLatestOrderDetailsIsCalled_ThenBadRequestShouldReturnAsync()
        {
            // Arrange
            _objectUnderTest.ModelState.AddModelError("Email", "isInvalid");

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetailsAsync(new CustomerIdentity());

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
            var result = await _objectUnderTest.GetLatestOrderDetailsAsync(identity);

            // Assert
            _trackingService.Verify(x => x.GetLastOrderDeliveryDetails("this@email.com", "someid"));
        }

        [Test]
        public async Task GivenGetLastOrderDeliveryDetailsIsCalled_WhenNullReturns_Then500ShouldReturnAsync()
        {
            // Arrange
            _trackingService
                .Setup(x => x.GetLastOrderDeliveryDetails(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((LatestOrderInfo)null);

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetailsAsync(new CustomerIdentity());

            // Assert
            Assert.That(((StatusCodeResult)result.Result).StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task GivenGetLastOrderDeliveryDetailsIsCalled_WhenCustomerIdentityInvalidExceptionIsThrown_ThenBadRequestShouldReturnAsync()
        {
            // Arrange
            _trackingService
                .Setup(x => x.GetLastOrderDeliveryDetails(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new CustomerIdentityInvalidException());

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetailsAsync(new CustomerIdentity());

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
        }


        [Test]
        public async Task GivenGetLastOrderDeliveryDetailsIsCalled_WhenAnyOtherExceptionIsThrown_Then500ReturnAsync()
        {
            // Arrange
            _trackingService
                .Setup(x => x.GetLastOrderDeliveryDetails(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _objectUnderTest.GetLatestOrderDetailsAsync(new CustomerIdentity());

            // Assert
            Assert.That(((StatusCodeResult)result.Result).StatusCode, Is.EqualTo(500));
        }
    }
}
