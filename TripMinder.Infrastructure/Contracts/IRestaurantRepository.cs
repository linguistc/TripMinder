using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IRestaurantRepository : IRepositoryAsync<Restaurant>
    {
        public Task<List<Restaurant>> GetRestaurantsListAsync();
        
        public Task<List<Restaurant>> GetRestaurantsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);

        public Task<List<Restaurant>> GetRestaurantsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        
        public Task<List<Restaurant>> GetRestaurantsListByClassIdAsync(int classId, CancellationToken cancellationToken = default);

        public Task<List<Restaurant>> GetRestaurantsListByFoodTypeIdAsync(int foodTypeId, CancellationToken cancellationToken = default);
        
        public Task<List<Restaurant>> GetRestaurantsListByRatingAsync(double rating, CancellationToken cancellationToken = default);
        
        public Task<List<Restaurant>> GetRestaurantsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default);
        
        public Task<List<Restaurant>> GetRestaurantsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default);
        
    }
}
