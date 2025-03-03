using Microsoft.Extensions.DependencyInjection;
using TripMinder.Service.Contracts;
using TripMinder.Service.Implementations;

namespace TripMinder.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependecies(this IServiceCollection services)
        {
            services.AddTransient<IAccomodationService, AccomodationService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddTransient<IEntertainmentService, EntertainmentService>();
            services.AddTransient<ITourismAreaService, TourismAreaService>();

            return services;
        }
    }
}
