using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Api.Models;
using Orders.Api.Services.Services.Interfaces;
using System;

namespace Orders.Api.Controllers
{
    [ApiController]
    public class OrdersDetailsController
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

        [HttpGet]
        public ActionResult<LatestOrderDto> GetLatestOrderDetails()
        {
            throw new NotImplementedException();
        }
    }
}
