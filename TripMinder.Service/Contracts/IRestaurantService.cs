using TripMinder.Data.Entities;
using TripMinder.Data.Enums;

namespace TripMinder.Service.Contracts
{
    public interface IRestaurantService
    {
        public Task<List<Restaurant>> GetRestaurantsListAsync();
        public Task<List<Restaurant>> GetRestaurantsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<Restaurant>> GetRestaurantsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        public Task<Restaurant> GetRestaurantByIdWithIncludeAsync(int id);
        
        public Task<List<Restaurant>> GetRestaurantsListByClassIdAsync(int classId, CancellationToken cancellationToken = default);
        public Task<List<Restaurant>> GetRestaurantsListByFoodTypeIdAsync(int foodTypeId, CancellationToken cancellationToken = default);
        public Task<List<Restaurant>> GetRestaurantsListByRatingAsync(double rating, CancellationToken cancellationToken = default);
        public Task<List<Restaurant>> GetRestaurantsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default);
        public Task<List<Restaurant>> GetRestaurantsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default);
        
        public Task<Restaurant> GetRestaurantByIdAsync(int id);
        public Task<string> CreateAsync(Restaurant newRestaurant);
        public Task<string> UpdateAsync(Restaurant restaurant);
        public Task<string> DeleteAsync(Restaurant restaurant);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
        // public IQueryable<Restaurant> FilterRestaurantPaginatedQuerable(RestaurantOrderingEnum orderingEnum, string search);
    }
}
