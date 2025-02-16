using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IRestaurantService
    {
        public Task<List<Restaurant>> GetAllRestaurantsAsync();
        public Task<Restaurant> GetRestaurantByIdAsync(int id);
    }
}
