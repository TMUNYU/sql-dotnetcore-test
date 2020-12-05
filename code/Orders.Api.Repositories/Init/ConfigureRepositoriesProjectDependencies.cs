using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Api.Repositories.Models;
using Orders.Api.Repositories.Models.Configuration;
using Orders.Api.Repositories.Repositories.Implementations;
using Orders.Api.Repositories.Repositories.Interfaces;

namespace Orders.Api.Repositories.Init
{
    public static class ConfigureRepositoriesProjectDependencies
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDbContext(services, configuration);
            ConfigureCustomerRepoClient(services, configuration);
            ConfigureOrderRepositoryClient(services);
        }

        private static void ConfigureOrderRepositoryClient(IServiceCollection services)
        {
            services.AddTransient<IOrdersRepository, OrdersRepository>();
        }

        private static void ConfigureCustomerRepoClient(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CustomerDetailsApiConfig>(configuration.GetSection("CustomerDetailsApi"));

            services.AddHttpClient<ICustomerDetailsRepository, CustomerDetailsRepository>();
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("OrdersDbConnectionString");
            services.AddDbContext<OrdersDBContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
