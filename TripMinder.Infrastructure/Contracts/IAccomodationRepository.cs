using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IAccomodationRepository : IRepositoryAsync<Accomodation>
    {
        public Task<List<Accomodation>> GetAccomodationsListAsync();
        public Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByClassIdAsync(int classId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByRatingAsync(double rating, CancellationToken cancellationToken = default);
        public Task<List<Accomodation>> GetAccomodationsListByNumOfBedsAsync(short numberOfBeds, CancellationToken cancellationToken = default);
        
        public Task<List<Accomodation>> GetAccomodationsListByNumOfAdultsAsync(short numberOfAdults, CancellationToken cancellationToken = default);

        public Task<List<Accomodation>> GetAccomodationsListLessThanPriceAsync(double price,
            CancellationToken cancellationToken = default);

        public Task<List<Accomodation>> GetAccomodationsListMoreThanPriceAsync(double price,
            CancellationToken cancellationToken = default);


    }
}
