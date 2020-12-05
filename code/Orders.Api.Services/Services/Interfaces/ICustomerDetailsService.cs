using Orders.Api.Services.Models.DomainModels;
using System.Threading.Tasks;

namespace Orders.Api.Services.Services.Interfaces
{
    public interface ICustomerDetailsService
    {
        public Task<CustomerDetailsInfo> GetCustomerDetailsByEmailAsync(string email, string customerId);
    }
}
