using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Models.Configuration;
using Orders.Api.Repositories.Repositories.Interfaces;
using System;
using System.Net.Http;

namespace Orders.Api.Repositories.Repositories.Implementations
{
    public class CustomerDetailsRepository : ICustomerDetailsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly CustomerDetailsApiConfig _apiConfiguration;

        public CustomerDetailsRepository(HttpClient httpClient, ILogger<CustomerDetailsRepository> logger, IOptions<CustomerDetailsApiConfig> apiConfiguration)
        {
            _httpClient = httpClient;
            _apiConfiguration = apiConfiguration.Value;
        }

        public CustomerDetails GetCustomerDetailsByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
