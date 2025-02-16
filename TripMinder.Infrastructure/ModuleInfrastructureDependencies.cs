using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Repositories;

namespace TripMinder.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));


            services.AddTransient<IAccomodationRepository, AccomodationRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IEntertainmentRepository, EntertainmentRepository>();
            services.AddTransient<ITourismAreaRepository, TourismAreaRepository>();


            return services;
        }
    }
}
