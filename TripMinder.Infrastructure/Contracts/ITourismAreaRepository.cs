using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface ITourismAreaRepository : IRepositoryAsync<TourismArea>
    {
        public Task<List<TourismArea>> GetAllTourismAreasAsync();

        public Task<List<TourismArea>> GetTourismAreasByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<TourismArea>> GetTourismAreasByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
    }
}
