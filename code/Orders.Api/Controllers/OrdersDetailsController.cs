using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Api.Models.Dtos.Request;
using Orders.Api.Models.Response;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Api.Controllers
{
    [Route("api/track")]
    [ApiController]
    public class OrdersDetailsController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerDetailsService _customerDetailsService;
        private readonly ILogger<OrdersDetailsController> _logger;

        public OrdersDetailsController(IOrderService orderService, ICustomerDetailsService customerDetailsService, ILogger<OrdersDetailsController> logger)
        {
            _orderService = orderService;
            _customerDetailsService = customerDetailsService;
            _logger = logger;
        }

        [HttpPost("lastorder")]
        public async Task<ActionResult<LatestOrderDto>> GetLatestOrderDetails(CustomerIdentity identity)
        {
            if (identity == null || !ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var customer = await _customerDetailsService.GetCustomerDetailsByEmailAsync(identity.User, identity.CustomerId);

            if (customer == null)
            {
                return new BadRequestResult();
            }

            var orderDetails = await _orderService.GetOrderByEmailAsync(identity.User, identity.CustomerId);

            return ConstructResponse(customer, orderDetails);
        }

        private ActionResult<LatestOrderDto> ConstructResponse(CustomerDetailsInfo customer, OrderInfo orderDetails)
        {
            return new LatestOrderDto
            {
                Customer = ConstructCustomer(customer),
                Order = ConstructOrders(orderDetails, customer)
            };
        }

        private OrderSummaryDto ConstructOrders(OrderInfo orderDetails, CustomerDetailsInfo customer)
        {
            if (orderDetails == null)
            {
                return null;
            }

            return new OrderSummaryDto
            {
                DeliveryAddress = ConstructAddress(customer),
                DeliveryExpected = (orderDetails.DeliveryExpected?.ToString("dd-MM-yyyy")) ?? "Not known",
                OrderDate = orderDetails.OrderDate.ToString("dd-MM-yyyy"),
                OrderItems = ConstructOrderItems(orderDetails.OrderItems),
                OrderNumber = orderDetails.OrderId
            };
        }

        private IEnumerable<OrderItemSummaryDto> ConstructOrderItems(IEnumerable<OrderItemInfo> orderItems)
        {
            return orderItems.Select(x => new OrderItemSummaryDto
            {
                PriceEach = x.UnitPrice,
                Quantity = x.Quantity,
                Product = x.Product.ProductName
            });
        }

        private string ConstructAddress(CustomerDetailsInfo customer)
        {
            var addressFragments = new[] {
                customer.HouseNumber,
                customer.Street,
                customer.Town,
                customer.Postcode
            }.Where(x=>!string.IsNullOrWhiteSpace(x));

            return string.Join(",", addressFragments);
        }

        private CustomerNamesDto ConstructCustomer(CustomerDetailsInfo customer)
        {
            return new CustomerNamesDto
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }
    }
}
