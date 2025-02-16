using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TripMinder.Service.Contracts;

namespace TripMinder.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependecies(this IServiceCollection services)
        {
            // Mediator Configuration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.GetExecutingAssembly()));

            // Auto Mapper Configuration
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            return services;
        }

    }
}
