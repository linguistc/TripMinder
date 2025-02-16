using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class TourismAreaService : ITourismAreaService
    {
        #region Fields
        private readonly ITourismAreaRepository repository;

        #endregion

        #region Constructors
        public TourismAreaService(ITourismAreaRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Functions
        public async Task<List<TourismArea>> GetAllTourismAreasAsync()
        {
            return await this.repository.GetAllTourismAreasAsync();
        }

        public async Task<TourismArea> GetTourismAreaByIdAsync(int id)
        {
            var tourism = this.repository.GetTableNoTracking()
                                    .Include(t => t.Description)
                                    .Include(t => t.PlaceCategory)
                                    .Include(t => t.Class)
                                    .Include(t => t.Zone)
                                    .Include(t => t.Location)
                                    .Where(t => t.Id.Equals(id))
                                    .FirstOrDefault();

            return tourism;
        }

        #endregion
    }
}
