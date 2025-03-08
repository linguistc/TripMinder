using TripMinder.Data.Entities;
using TripMinder.Data.Enums;

namespace TripMinder.Service.Contracts
{
    public interface IRestaurantService
    {
        public Task<List<Restaurant>> GetAllRestaurantsAsync();
        public Task<Restaurant> GetRestaurantByIdWithIncludeAsync(int id);
        public Task<Restaurant> GetRestaurantByIdAsync(int id);
        public Task<string> CreateAsync(Restaurant newRestaurant);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
        public Task<string> UpdateAsync(Restaurant restaurant);
        public Task<string> DeleteAsync(Restaurant restaurant);
        // public IQueryable<Restaurant> FilterRestaurantPaginatedQuerable(RestaurantOrderingEnum orderingEnum, string search);
    }
}
