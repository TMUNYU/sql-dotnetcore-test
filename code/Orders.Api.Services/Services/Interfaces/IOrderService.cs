using Orders.Api.Services.Models.DomainModels;
using System.Threading.Tasks;

namespace Orders.Api.Services.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderInfo> GetOrderByEmailAsync(string email, string customerId);
    }
}
