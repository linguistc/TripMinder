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
        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var result = await this.restaurants.Include(r => r.Description)
                                         .Include(r => r.Zone)
                                         .Include(r => r.Class)
                                         .Include(r => r.Location)
                                         .Include(r => r.PlaceCategory)
                                         .ToListAsync();

            return result;                     
        }

        #endregion



    }
}
