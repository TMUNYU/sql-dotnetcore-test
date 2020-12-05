using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Api.Repositories.Repositories.Implementations
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersDBContext _ordersDBContext;
        private readonly ILogger<OrdersRepository> _logger;

        public OrdersRepository(OrdersDBContext ordersDBContext, ILogger<OrdersRepository> logger)
        {
            _ordersDBContext = ordersDBContext;
            _logger = logger;
        }

        public Task<Order> GetOrdersByCustomerIdLastestOnlyAsync(string customerId)
        {
            var order = _ordersDBContext.Orders
                .Include(x=>x.Orderitems)
                .ThenInclude(x=>x.Product)
                .Where(x=>x.Customerid == customerId)
                .OrderByDescending(x => x.Orderdate).FirstOrDefault();
            return Task.FromResult(order);
        }
    }
}
