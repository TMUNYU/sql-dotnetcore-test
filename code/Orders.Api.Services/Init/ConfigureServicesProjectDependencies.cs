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

            ConfigureTrackingService(services);
        }

        private static void ConfigureTrackingService(IServiceCollection services)
        {
            services.AddTransient<ITrackingService, TrackingService>();
        }
    }
}
