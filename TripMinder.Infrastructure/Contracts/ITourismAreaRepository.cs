using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface ITourismAreaRepository : IRepositoryAsync<TourismArea>
    {
        public Task<List<TourismArea>> GetAllTourismAreasAsync();

    }
}
