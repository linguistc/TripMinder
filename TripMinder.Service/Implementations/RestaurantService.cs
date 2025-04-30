using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Extentions;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class RestaurantService : IRestaurantService
    {

        #region Fields
        private readonly IRestaurantRepository _repository;

        #endregion

        #region Constructors
        public async Task<double?> GetMinimumPriceAsync(CancellationToken cancellationToken = default)
        {
            return await this._repository.GetMinimumPriceAsync(cancellationToken);
        }
        public RestaurantService(IRestaurantRepository repository)
        {
            this._repository = repository;
        }

        

        #endregion

        #region Functions
        public async Task<List<Restaurant>> GetRestaurantsListAsync()
        {
            return await _repository.GetRestaurantsListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurantsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListByZoneIdAsync(zoneId, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListByGovernorateIdAsync(governorateId, cancellationToken);
        }

        public async Task<Restaurant> GetRestaurantByIdWithIncludeAsync(int id)
        {
            var restaurant = this._repository.GetTableNoTracking()
                .Include(r => r.FoodCategory)
                .Include(r => r.PlaceType)
                .Include(r => r.Class)
                .Include(r => r.Zone)
                .Include(r => r.Zone.Governorate)
                .FirstOrDefault(r => r.Id == id);

            return restaurant;
        }

        public async Task<List<Restaurant>> GetRestaurantsListByClassIdAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListByClassIdAsync(classId, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByFoodTypeIdAsync(int foodTypeId, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListByFoodTypeIdAsync(foodTypeId, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListByRatingAsync(rating, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListMoreThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await _repository.GetRestaurantsListLessThanPriceAsync(price, cancellationToken);
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await this._repository.GetByIdAsync(id);

            return restaurant;
        }

        public Task<bool> IsNameArExist(string nameAr)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameEnExist(string nameEn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateAsync(Restaurant restaurant)
        {
            await this._repository.UpdateAsync(restaurant);
            return "Updated";

        }
        
        public async Task<string> CreateAsync(Restaurant newRestaurant)
        {
            await this._repository.CreateAsync(newRestaurant);
            return "Created";
        }

        public async Task<string> DeleteAsync(Restaurant restaurant)
        {
            var trans = this._repository.BeginTransaction();

            try
            {
                await this._repository.DeleteAsync(restaurant);
                await trans.CommitAsync();
                return "Deleted";
            }
            catch
            {
                await trans.RollbackAsync();
                return "Failed";
            }
        }
        
        #endregion

    }
}
