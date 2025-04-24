using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface ITourismAreaRepository : IRepositoryAsync<TourismArea>
    {
        public Task<List<TourismArea>> GetAllTourismAreasAsync();

        public Task<List<TourismArea>> GetTourismAreasListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<TourismArea>> GetTourismAreasListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        
        public Task<List<TourismArea>> GetTourismAreasListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default);

        public Task<List<TourismArea>> GetTourismAreasListByRatingAsync(double rating, CancellationToken cancellationToken = default);
        
        public Task<List<TourismArea>> GetTourismAreasListByClassIdAsync(int classId, CancellationToken cancellationToken = default);
        
        public Task<List<TourismArea>> GetTourismAreasListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default);
        
        public Task<List<TourismArea>> GetTourismAreasListLessThanPriceAsync(double price, CancellationToken cancellationToken = default);

    }
}
