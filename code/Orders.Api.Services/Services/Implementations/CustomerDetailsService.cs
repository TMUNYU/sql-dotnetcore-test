using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Repositories.Interfaces;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;
using System.Threading.Tasks;

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

        public Task<CustomerDetailsInfo> GetCustomerDetailsByEmailAsync(string email, string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
