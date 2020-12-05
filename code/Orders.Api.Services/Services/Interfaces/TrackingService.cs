using Orders.Api.Services.Models.DomainModels;
using System.Threading.Tasks;

namespace Orders.Api.Services.Services.Interfaces
{
    public interface ITrackingService
    {
        public Task<LatestOrderInfo> GetLastOrderDeliveryDetails(string email, string customerId);
    }
}
