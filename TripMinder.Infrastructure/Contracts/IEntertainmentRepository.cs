using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IEntertainmentRepository : IRepositoryAsync<Entertainment>
    {
        public Task<List<Entertainment>> GetEntertainmentsListAsync();
        
        public Task<List<Entertainment>> GetEntertainmentsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);

        public Task<List<Entertainment>> GetEntertainmentsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        
        public Task<List<Entertainment>> GetEntertainmentsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default);

        public Task<List<Entertainment>> GetEntertainmentsListByClassIdAsync(int classId, CancellationToken cancellationToken = default);

        public Task<List<Entertainment>> GetEntertainmentsListByRatingAsync(double rating, CancellationToken cancellationToken = default);

        public Task<List<Entertainment>> GetEntertainmentsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default);

        public Task<List<Entertainment>> GetEntertainmentsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default);
        

    }
}
