using Microsoft.Extensions.DependencyInjection;
using TripMinder.Infrastructure.Contracts;
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

            services.AddTransient<IAccomodationClassService, AccomodationClassService>();
            services.AddTransient<IAccomodationTypeService, AccomodationTypeService>();
            services.AddTransient<IEntertainmentClassService, EntertainmentClassService>();
            services.AddTransient<IEntertainmentTypeService, EntertainmentTypeService>();
            services.AddTransient<IFoodCategoryService, FoodCategoryService>();
            services.AddTransient<IPlaceTypeService, PlaceTypeService>();
            services.AddTransient<IRestaurantClassService, RestaurantClassService>();
            services.AddTransient<ITourismAreaClassService, TourismAreaClassService>();
            services.AddTransient<ITourismTypeService, TourismTypeService>();
            services.AddTransient<IZoneService, ZoneService>();
            services.AddTransient<IGovernorateService, GovernorateService>();

            return services;
        }
    }
}
