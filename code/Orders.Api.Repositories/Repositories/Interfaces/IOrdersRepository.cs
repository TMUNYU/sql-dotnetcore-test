using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Orders.Api.Repositories.Models;

namespace Orders.Api.Repositories.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<Order> GetOrdersByCustomerIdLastestOnlyAsync(string customerId);
    }
}