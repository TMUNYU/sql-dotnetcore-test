using Orders.Api.Repositories.Models;
using System.Threading.Tasks;

namespace Orders.Api.Repositories.Repositories.Interfaces
{
    public interface ICustomerDetailsRepository
    {
        Task<CustomerDetails> GetCustomerDetailsByEmailAsync(string email);
    }
}