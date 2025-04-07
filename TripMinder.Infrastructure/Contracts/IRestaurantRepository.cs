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
    }
}
