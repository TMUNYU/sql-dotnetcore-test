using Orders.Api.Repositories.Models;

namespace Orders.Api.Repositories.Repositories.Interfaces
{
    public interface ICustomerDetailsRepository
    {
        CustomerDetails GetCustomerDetailsByEmail(string email);
    }
}