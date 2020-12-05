using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Models.Configuration;
using Orders.Api.Repositories.Models.Request;
using Orders.Api.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<CustomerDetails> GetCustomerDetailsByEmailAsync(string email)
        {
            _httpClient.BaseAddress = new Uri(_apiConfiguration.BaseUrl);

            var requestUri = $"{_apiConfiguration.GetUserDetailsEndpoint}?code={_apiConfiguration.Key}&email={email}";

            var response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CustomerDetails>(responseContent);

            return data;
        }

        private string ConvertToDto(string email)
        {
            return JsonConvert.SerializeObject(new CustomerDetailsRequestDto
            {
                Email = email
            });
        }
    }
}
