using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Repositories.Interfaces;
using Orders.Api.Services.Exceptions;
using Orders.Api.Services.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Api.Tests.Orders.Api.Services.Tests.TrackingServiceTests
{
    [TestFixture]
    public class GetLastOrderDeliveryDetailsTests
    {
        private Mock<ICustomerDetailsRepository> _customerDetailsRepository;
        private Mock<IOrdersRepository> _ordersRepository;

        private TrackingService _objectUnderTest;

        [SetUp]
        public void Setup()
        {
            _customerDetailsRepository = new Mock<ICustomerDetailsRepository>();
            _ordersRepository = new Mock<IOrdersRepository>();

            _objectUnderTest = new TrackingService(_customerDetailsRepository.Object, _ordersRepository.Object, Mock.Of<ILogger<TrackingService>>());
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void GivenEmailIsNullOrEmpty_WhenGetLastOrderDeliveryDetailsIsCalled_ThenArgumentNullShouldBeThrown(string email)
        {
            // Assert
            Assert.That(async () =>
            {
                // Act
                await _objectUnderTest.GetLastOrderDeliveryDetails(email, "someId");
            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void GivenCustomerIdNullOrEmpty_WhenGetLastOrderDeliveryDetailsIsCalled_ThenArgumentNullShouldBeThrown(string customerId)
        {
            // Assert
            Assert.That(async () =>
            {
                // Act
                await _objectUnderTest.GetLastOrderDeliveryDetails("someemail", customerId);
            }, Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public async Task GivenEmailAndIdAreValid_WhenCustomerServiceIsCalled_ThenSameEmailShouldBePassedToItAsync()
        {
            // Arrange
            const string email = "someEmail0";
            EnsureCustomer("someId");

            // Act
            await _objectUnderTest.GetLastOrderDeliveryDetails(email, "someId");

            // Assert
            _customerDetailsRepository.Verify(x => x.GetCustomerDetailsByEmailAsync(email), Times.Once);
        }



        [Test]
        public void GivenCustomerDetailsAreReturned_WhenReturnedDetailsDoNotHaveTheSameCustomerIdAsPassedToMethodUnderTest_ThenCustomerIdentityInvalidExceptionShouldBeThrown()
        {
            // Arrange
            _customerDetailsRepository
                .Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new CustomerDetails
                {
                    CustomerId = "id being tested for - diff"
                });

            // Assert
            Assert.That(async () =>
            {
                // Act
                await _objectUnderTest.GetLastOrderDeliveryDetails("someEmail", "id being tested for");
            }, Throws.InstanceOf<CustomerIdentityInvalidException>());
        }

        [Test]
        public void GivenCustomerDetailsAreReturned_WhenReturnedDetailsHaveTheSameCustomerIdAsPassedToMethodUnderTest_ThenNoExceptionShouldBeThrown()
        {
            // Arrange
            _customerDetailsRepository
                .Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new CustomerDetails
                {
                    CustomerId = "id being tested for"
                });

            // Assert
            Assert.That(async () =>
            {
                // Act
                await _objectUnderTest.GetLastOrderDeliveryDetails("someEmail", "id being tested for");
            }, Throws.Nothing);
        }

        [Test]
        public async Task GivenUserIdComboIsCorrect_WhenOrderIsFetched_ThenCustomeridPassedToMethodUnderTestShouldBePassedToGetOrdersByCustomerId()
        {
            // Arrange
            const string customerId = "the expectedId";
            EnsureCustomer(customerId);

            // Act
            await _objectUnderTest.GetLastOrderDeliveryDetails("someemail", customerId);

            // Assert
            _ordersRepository.Verify(x => x.GetOrdersByCustomerIdLastestOnlyAsync(customerId), Times.Once);
        }

        [Test]
        public async Task GivenUserIsValid_WhenUserHasNoOrders_ThenCustomerDetailsShouldBeInRetuurnDtoWithNullForOrderDetails()
        {
            // Arrange
            const string customerId = "the expectedId";
            _ordersRepository
                .Setup(x => x.GetOrdersByCustomerIdLastestOnlyAsync(It.IsAny<string>()))
                .ReturnsAsync((Order)null);

            _customerDetailsRepository
                .Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new CustomerDetails
                {
                    CustomerId = customerId,
                    FirstName = "John",
                    LastName = "Doe"
                });

            // Act
            var result = await _objectUnderTest.GetLastOrderDeliveryDetails("someemail", customerId);

            // Assert
            Assert.Multiple(()=> {
                Assert.That(result.Order, Is.Null);
                Assert.That(result.Customer.FirstName, Is.EqualTo("John"));
                Assert.That(result.Customer.LastName, Is.EqualTo("Doe"));
            });
        }

        [Test]
        public async Task GivenAnOrderIsFound_WhenOneOfItsProductsIsAGift_ThenGiftShouldBeReturnedInTheProductName()
        {
            // Arrange
            const string customerId = "the expectedId";
            _ordersRepository
                .Setup(x => x.GetOrdersByCustomerIdLastestOnlyAsync(It.IsAny<string>()))
                .ReturnsAsync(new Order
                {
                    Containsgift = true,
                    Orderitems = new[]
                    {
                        new Orderitem
                        {
                            Product = new Product
                            {
                                Productname = "ps5"
                            }
                        },
                        new Orderitem
                        {
                            Product = new Product
                            {
                                Productname = "M3 toy car"
                            }
                        },
                    }
                });
            EnsureCustomer(customerId);

            // Act
            var result = await _objectUnderTest.GetLastOrderDeliveryDetails("someemail", customerId);

            // Assert
            Assert.That(result.Order.OrderItems.All(x => x.Product == "Gift"), Is.True);
        }

        [Test]
        public async Task GivenAnOrderIsFound_WhenProductIsReturned_ThenUnitPriceMustBeCalculated()
        {
            // Arrange
            const string customerId = "the expectedId";
            _ordersRepository
                .Setup(x => x.GetOrdersByCustomerIdLastestOnlyAsync(It.IsAny<string>()))
                .ReturnsAsync(new Order
                {
                    Containsgift = false,
                    Orderitems = new[]
                    {
                        new Orderitem
                        {
                            Product = new Product
                            {
                                Productname = "p1"
                            },
                            Price = Convert.ToDecimal(10.57),
                            Quantity = 3
                        },
                        new Orderitem
                        {
                            Product = new Product
                            {
                                Productname = "p2"
                            },
                            Price = Convert.ToDecimal(111.73),
                            Quantity = 13
                        },
                    }
                });
            EnsureCustomer(customerId);

            // Act
            var result = await _objectUnderTest.GetLastOrderDeliveryDetails("someemail", customerId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Order.OrderItems.First(x => x.Product == "p1").PriceEach, Is.EqualTo(3.52));
                Assert.That(result.Order.OrderItems.First(x => x.Product == "p2").PriceEach, Is.EqualTo(8.59));
            });
        }

        private void EnsureCustomer(string someid)
        {
            _customerDetailsRepository
                .Setup(x => x.GetCustomerDetailsByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new CustomerDetails
                {
                    CustomerId = someid,
                    FirstName = "John",
                    LastName = "Doe"
                });
        }
    }
}
