using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Api.Models.Dtos.Request;
using Orders.Api.Services.Exceptions;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Orders.Api.Controllers
{
    [Route("api/track")]
    [ApiController]
    public class OrdersDetailsController : Controller
    {
        private readonly ITrackingService _trackingService;
        private readonly ILogger<OrdersDetailsController> _logger;

        public OrdersDetailsController(ITrackingService trackingService, ILogger<OrdersDetailsController> logger)
        {
            _trackingService = trackingService;
            _logger = logger;
        }

        [HttpPost("lastorder")]
        public async Task<ActionResult<LatestOrderInfo>> GetLatestOrderDetailsAsync(CustomerIdentity identity)
        {
            if (identity == null || !ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            try
            {
                var customer = await _trackingService.GetLastOrderDeliveryDetails(identity.User, identity.CustomerId);

                if (customer == null)
                {
                    _logger.LogError("Tracking service unexpectedly returned null for user id {userId}.", identity.CustomerId);
                    return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                }

                return new OkObjectResult(customer);
            }
            catch(CustomerIdentityInvalidException ciie)
            {
                _logger.LogError(ciie, "Invalid user and customer id combination for user {userId} requested.", identity.CustomerId);
                return new BadRequestResult();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve tracking information for user id {userId}.", identity.CustomerId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }     
        }

        //private ActionResult<LatestOrderInfo> ConstructResponse(CustomerDetailsInfo customer, OrderInfo orderDetails)
        //{
        //    return new LatestOrderInfo
        //    {
        //        Customer = ConstructCustomer(customer),
        //        Order = ConstructOrders(orderDetails, customer)
        //    };
        //}

        //private OrderSummary ConstructOrders(OrderInfo orderDetails, CustomerDetailsInfo customer)
        //{
        //    if (orderDetails == null)
        //    {
        //        return null;
        //    }

        //    return new OrderSummary
        //    {
        //        DeliveryAddress = ConstructAddress(customer),
        //        DeliveryExpected = (orderDetails.DeliveryExpected?.ToString("dd-MM-yyyy")) ?? "Not known",
        //        OrderDate = orderDetails.OrderDate.ToString("dd-MM-yyyy"),
        //        OrderItems = ConstructOrderItems(orderDetails.OrderItems),
        //        OrderNumber = orderDetails.OrderId
        //    };
        //}

        //private IEnumerable<OrderItemSummary> ConstructOrderItems(IEnumerable<OrderItemInfo> orderItems)
        //{
        //    return orderItems.Select(x => new OrderItemSummary
        //    {
        //        PriceEach = x.UnitPrice,
        //        Quantity = x.Quantity,
        //        Product = x.Product.ProductName
        //    });
        //}

        //private string ConstructAddress(CustomerDetailsInfo customer)
        //{
        //    var addressFragments = new[] {
        //        customer.HouseNumber,
        //        customer.Street,
        //        customer.Town,
        //        customer.Postcode
        //    }.Where(x=>!string.IsNullOrWhiteSpace(x));

        //    return string.Join(",", addressFragments);
        //}

        //private CustomerNames ConstructCustomer(CustomerDetailsInfo customer)
        //{
        //    return new CustomerNames
        //    {
        //        FirstName = customer.FirstName,
        //        LastName = customer.LastName
        //    };
        //}
    }
}
