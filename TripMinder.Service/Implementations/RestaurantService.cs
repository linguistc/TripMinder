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
        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            return await repository.GetAllRestaurantsAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdWithIncludeAsync(int id)
        {
            var restaurant = this.repository.GetTableNoTracking()
                .Include(r => r.Description)
                .Include(r => r.PlaceCategory)
                .Include(r => r.Class)
                .Include(r => r.Zone)
                .Include(r => r.Location)
                .FirstOrDefault(r => r.Id == id);

            return restaurant;
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
