using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Orders.Api.Repositories.Models;

namespace Orders.Api.Repositories.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrdersByCustomerId(string customerId, Expression<Func<Order, bool>> predicate);
    }
}