using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface ITourismAreaService
    {
        public Task<List<TourismArea>> GetAllTourismAreasAsync();
        public Task<TourismArea> GetTourismAreaByIdAsync(int id);
    }
}
