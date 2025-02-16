using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
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

        #endregion

    }
}
