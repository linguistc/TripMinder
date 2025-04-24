using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class RestaurantService : IRestaurantService
    {

        #region Fields
        private readonly IRestaurantRepository repository;

        #endregion

        #region Constructors
        public RestaurantService(IRestaurantRepository repository)
        {
            this.repository = repository;
        }

        

        #endregion

        #region Functions
        public async Task<List<Restaurant>> GetRestaurantsListAsync()
        {
            return await repository.GetRestaurantsListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurantsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await repository.GetRestaurantsListByZoneIdAsync(zoneId, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await repository.GetRestaurantsListByGovernorateIdAsync(governorateId, cancellationToken);
        }

        public async Task<Restaurant> GetRestaurantByIdWithIncludeAsync(int id)
        {
            var restaurant = this.repository.GetTableNoTracking()
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
            return await repository.GetRestaurantsListByClassIdAsync(classId, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByFoodTypeIdAsync(int foodTypeId, CancellationToken cancellationToken = default)
        {
            return await repository.GetRestaurantsListByFoodTypeIdAsync(foodTypeId, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await repository.GetRestaurantsListByRatingAsync(rating, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await repository.GetRestaurantsListMoreThanPriceAsync(price, cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await repository.GetRestaurantsListLessThanPriceAsync(price, cancellationToken);
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await this.repository.GetByIdAsync(id);

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
            await this.repository.UpdateAsync(restaurant);
            return "Updated";

        }
        
        public async Task<string> CreateAsync(Restaurant newRestaurant)
        {
            await this.repository.CreateAsync(newRestaurant);
            return "Created";
        }

        public async Task<string> DeleteAsync(Restaurant restaurant)
        {
            var trans = this.repository.BeginTransaction();

            try
            {
                await this.repository.DeleteAsync(restaurant);
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
