using Orders.Api.Services.Models.DomainModels;

namespace Orders.Api.Services.Services.Interfaces
{
    public interface ICustomerDetailsService
    {
        public OrderInfo GetLatestOrderByCustomerId(string email);
    }
}
