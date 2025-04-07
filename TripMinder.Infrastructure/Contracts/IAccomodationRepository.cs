using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IAccomodationRepository : IRepositoryAsync<Accomodation>
    {
        public Task<List<Accomodation>> GetAccomodationsListAsync();
        public Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);

    }
}
