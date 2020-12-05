using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Api.Repositories.Init;
using Orders.Api.Services.Services.Implementations;
using Orders.Api.Services.Services.Interfaces;

namespace Orders.Api.Services.Init
{
    public static class ConfigureServicesProjectDependencies
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureRepositoriesProjectDependencies.Configure(services, configuration);

            ConfigureCustomerDetailsService(services);
            ConfigureOrdersService(services);
        }

        private static void ConfigureOrdersService(IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
        }

        private static void ConfigureCustomerDetailsService(IServiceCollection services)
        {
            services.AddTransient<ICustomerDetailsService, CustomerDetailsService>();
        }
    }
}
