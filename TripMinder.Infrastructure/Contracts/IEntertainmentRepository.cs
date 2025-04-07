using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IEntertainmentRepository : IRepositoryAsync<Entertainment>
    {
        public Task<List<Entertainment>> GetEntertainmentsListAsync();
        
        public Task<List<Entertainment>> GetEntertainmentsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);

        public Task<List<Entertainment>> GetEntertainmentsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);

    }
}
