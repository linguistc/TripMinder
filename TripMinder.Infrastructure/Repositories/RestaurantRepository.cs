using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories
{
    public class RestaurantRepository : RepositoryAsync<Restaurant>, IRestaurantRepository
    {
        #region Fields
        private readonly DbSet<Restaurant> restaurants;

        #endregion

        #region Constructors
        public RestaurantRepository(AppDBContext dbContext) : base(dbContext)
        {
            this.restaurants = dbContext.Set<Restaurant>();
        }


        #endregion

        #region Functions
        public async Task<List<Restaurant>> GetRestaurantsListAsync()
        {
            var result = await this.restaurants.Include(r => r.FoodCategory)
                                         .Include(r => r.Zone)
                                         .Include(r => r.Class)
                                         .Include(r => r.PlaceType)
                                         .ToListAsync();

            return result;                     
        }

        public async Task<List<Restaurant>> GetRestaurantsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await this.restaurants
                .Include(r => r.FoodCategory)
                .Include(r => r.Zone)
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(r => r.ZoneId == zoneId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Restaurant>> GetRestaurantsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await this.restaurants
                .Include(r => r.FoodCategory)
                .Include(r => r.Zone)
                .Include(r => r.Class)
                .Include(r => r.PlaceType)                
                .Where(r => r.Zone.GovernorateId == governorateId)
                .ToListAsync(cancellationToken);
        }
        #endregion



    }
}
