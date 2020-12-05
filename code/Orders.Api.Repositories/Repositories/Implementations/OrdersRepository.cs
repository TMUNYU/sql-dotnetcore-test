using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId, Expression<Func<Order, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
