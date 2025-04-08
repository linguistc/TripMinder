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

            var result = await this.tourismAreas.Include(r => r.TourismType)
                                         .Include(r => r.Zone)
                                         .Include(r => r.Class)
                                         .Include(r => r.PlaceType)
                                         .ToListAsync();

            return result;
        }

        public async Task<List<TourismArea>> GetTourismAreasByZoneIdAsync(int zoneId,
            CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(r => r.TourismType)
                .Include(r => r.Zone)
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(r => r.ZoneId == zoneId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasByGovernorateIdAsync(int governorateId,
            CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(r => r.TourismType)
                .Include(r => r.Zone)
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(r => r.Zone.GovernorateId == governorateId)
                .ToListAsync(cancellationToken);
        }

        #endregion


    }
}
