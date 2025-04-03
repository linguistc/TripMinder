using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IRestaurantClassService
{
    public Task<List<RestaurantClass>> GetRestaurantClassesListAsync();
    public Task<RestaurantClass> GetRestaurantClassByIdAsync(int id);
    public Task<bool> IsRestaurantClassIdExist(int id);
    public Task<string> CreateAsync(RestaurantClass restaurantClass);
    public Task<string> UpdateAsync(RestaurantClass restaurantClass);
    public Task<string> DeleteAsync(RestaurantClass restaurantClass);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}