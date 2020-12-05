﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Repositories.Interfaces;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;

namespace Orders.Api.Services.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrdersRepository ordersRepository, ILogger<OrderService> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public Task<OrderInfo> GetOrderByEmailAsync(string email, string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
