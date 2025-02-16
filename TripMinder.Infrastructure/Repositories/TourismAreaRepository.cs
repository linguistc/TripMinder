using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories
{
    public class TourismAreaRepository : RepositoryAsync<TourismArea>, ITourismAreaRepository
    {

        #region Fields
        private readonly DbSet<TourismArea> tourismAreas;

        #endregion

        #region Constructors
        public TourismAreaRepository(AppDBContext dbContext) : base(dbContext)
        {
            this.tourismAreas = dbContext.Set<TourismArea>();
        }

        #endregion

        #region Functions
        public async Task<List<TourismArea>> GetAllTourismAreasAsync()
        {

            var result = await this.tourismAreas.Include(r => r.Description)
                                         .Include(r => r.Zone)
                                         .Include(r => r.Class)
                                         .Include(r => r.Location)
                                         .Include(r => r.PlaceCategory)
                                         .ToListAsync();

            return result;
        }

        #endregion


    }
}
