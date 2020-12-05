using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Repositories.Interfaces;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;

namespace Orders.Api.Services.Services.Implementations
{
    public class CustomerDetailsService: ICustomerDetailsService
    {
        private readonly ICustomerDetailsRepository _customerDetailsRepository;
        private readonly ILogger<CustomerDetailsService> _logger;

        public CustomerDetailsService(ICustomerDetailsRepository customerDetailsRepository, ILogger<CustomerDetailsService> logger)
        {
            _customerDetailsRepository = customerDetailsRepository;
            _logger = logger;
        }

        public OrderInfo GetLatestOrderByCustomerId(string email)
        {
            throw new NotImplementedException();
        }
    }
}
