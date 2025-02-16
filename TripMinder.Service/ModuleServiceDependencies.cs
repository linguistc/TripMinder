using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TripMinder.Service.Contracts;

namespace TripMinder.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependecies(this IServiceCollection services)
        {
            services.AddTransient<IAccomodationService, IAccomodationService>();
            services.AddTransient<IRestaurantService, IRestaurantService>();
            services.AddTransient<IEntertainmentService, IEntertainmentService>();
            services.AddTransient<ITourismAreaService, ITourismAreaService>();

            return services;
        }
    }
}
