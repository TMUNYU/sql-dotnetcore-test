using Microsoft.Extensions.Logging;
using Orders.Api.Repositories.Repositories.Interfaces;
using Orders.Api.Services.Models.DomainModels;
using Orders.Api.Services.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Orders.Api.Services.Services.Implementations
{
    public class TrackingService: Interfaces.ITrackingService
    {
        private readonly ICustomerDetailsRepository _customerDetailsRepository;
        private readonly ILogger<TrackingService> _logger;

        public TrackingService(ICustomerDetailsRepository customerDetailsRepository, ILogger<TrackingService> logger)
        {
            _customerDetailsRepository = customerDetailsRepository;
            _logger = logger;
        }

        public Task<CustomerDetailsInfo> GetLastOrderDeliveryDetails(string email, string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
