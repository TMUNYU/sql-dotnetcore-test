using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Repositories.Interfaces;
using Orders.Api.Services.Exceptions;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Api.Services.Services.Implementations
{
    public class TrackingService: ITrackingService
    {
        private readonly ICustomerDetailsRepository _customerDetailsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<TrackingService> _logger;

        public TrackingService(ICustomerDetailsRepository customerDetailsRepository, IOrdersRepository ordersRepository, ILogger<TrackingService> logger)
        {
            _customerDetailsRepository = customerDetailsRepository;
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public async Task<LatestOrderInfo> GetLastOrderDeliveryDetails(string email, string customerId)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            var customerDetails = await _customerDetailsRepository.GetCustomerDetailsByEmailAsync(email);

            ThrowIfCustomerIdsDoNotMatch(customerDetails?.CustomerId, customerId, email);

            var orderDetails = await _ordersRepository.GetOrdersByCustomerIdLastestOnlyAsync(customerId);

            return new LatestOrderInfo
            {
                Customer = MapCustomerDetails(customerDetails),
                Order = MapOrderDetails(orderDetails, customerDetails)
            };
        }

        private OrderSummary MapOrderDetails(Order orderDetails, CustomerDetails customerDetails)
        {
            if (orderDetails == null)
            {
                return null;
            }

            return new OrderSummary()
            {
                OrderDate = EnsureDateFormate(orderDetails.Orderdate.GetValueOrDefault()),
                DeliveryExpected = EnsureDateFormate(orderDetails.Deliveryexpected.GetValueOrDefault()),
                OrderNumber = orderDetails.Orderid,
                DeliveryAddress = ConstructAddress(customerDetails),
                OrderItems = MapOrderItems(orderDetails)
            };
        }

        private string EnsureDateFormate(DateTime orderdate)
        {
            return orderdate.ToString("dd-MM-yyyy");
        }

        private string ConstructAddress(CustomerDetails customerDetails)
        {
            var addressFragment = new[]
            {
                customerDetails.HouseNumber,
                customerDetails.Street,
                customerDetails.Town,
                customerDetails.Postcode
            }.Where(x => !string.IsNullOrWhiteSpace(x));

            return string.Join(", ", addressFragment);
        }

        private IEnumerable<OrderItemSummary> MapOrderItems(Order orderDetails)
        {
            return orderDetails.Orderitems.Select(orderItem => new OrderItemSummary
            {
                Product = ResoveProductName(orderDetails, orderItem),
                Quantity = orderItem.Quantity.GetValueOrDefault(),
                PriceEach = CalculateUnitPrice(orderItem.Price.GetValueOrDefault(), orderItem.Quantity.GetValueOrDefault())
            });
        }

        private decimal CalculateUnitPrice(decimal price, int quantity)
        {
            if (quantity == 0)
            {
                return default;
            }

            return Math.Round(price / quantity, 2, MidpointRounding.AwayFromZero);
        }

        private static string ResoveProductName(Order orderDetails, Orderitem orderItem)
        {
            return orderDetails.Containsgift.Value ? "Gift" : orderItem.Product.Productname;
        }

        private CustomerNames MapCustomerDetails(CustomerDetails customerDetails)
        {
            return new CustomerNames
            {
                FirstName = customerDetails.FirstName,
                LastName = customerDetails.LastName
            };
        }

        private void ThrowIfCustomerIdsDoNotMatch(string actualId, string requestedId, string email)
        {
            if (actualId != null && actualId.Equals(requestedId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }                
            
            var errorMsg = $"requestedId={requestedId} is not equal to {actualId} we have in Db for the email {email}";
            throw new CustomerIdentityInvalidException(errorMsg);
        }
    }
}
