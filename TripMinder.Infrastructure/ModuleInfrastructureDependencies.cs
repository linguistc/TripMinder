using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;
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
            
            services.AddTransient<IAccomodationClassRepository, AccomodationClassRepository>();
            services.AddTransient<IAccomodationTypeRepository, AccomodationTypeRepository>();
            services.AddTransient<IEntertainmentClassRepository, EntertainmentClassRepository>();
            services.AddTransient<IEntertainmentTypeRepository, EntertainmentTypeRepository>();
            services.AddTransient<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddTransient<IPlaceTypeRepository, PlaceTypeRepository>();
            services.AddTransient<IRestaurantClassRepository, RestaurantClassRepository>();
            services.AddTransient<ITourismAreaClassRepository, TourismAreaClassRepository>();
            services.AddTransient<ITourismTypeRepository, TourismTypeRepository>();
            services.AddTransient<IZoneRepository, ZoneRepository>();

            services.AddTransient<IGovernorateRepository, GovernorateRepository>();
            // Add DataSeeder to DI
            services.AddScoped<DataSeeder>();
            
            return services;
        }
    }
}
