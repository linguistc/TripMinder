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
                                         .Include(t => t.Zone).AsNoTracking()
                                         .Include(t => t.Zone.Governorate).AsNoTracking()
                                         .Include(t => t.Class)
                                         .Include(t => t.PlaceType)
                                         .ToListAsync();

            return result;
        }

        public async Task<List<TourismArea>> GetTourismAreasByZoneIdAsync(int zoneId,
            CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.ZoneId == zoneId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TourismArea>> GetTourismAreasByGovernorateIdAsync(int governorateId,
            CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.Zone.GovernorateId == governorateId)
                .ToListAsync(cancellationToken);
        }

        
        
        
        public async Task<List<TourismArea>> GetTourismAreasByClassIdAsync(int classId,
            CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.ClassId == classId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<TourismArea>> GetTourismAreasByTypeIdAsync(int TypeId,
            CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.TourismTypeId == TypeId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<TourismArea>> GetTourismAreasByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.Rating > rating)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<TourismArea>> GetTourismAreasByPriceMoreThanAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.AveragePricePerAdult > price)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<TourismArea>> GetTourismAreasByPriceLessThanAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this.tourismAreas
                .Include(t => t.TourismType)
                .Include(t => t.Zone).AsNoTracking()
                .Include(t => t.Zone.Governorate).AsNoTracking()
                .Include(t => t.Class)
                .Include(t => t.PlaceType)
                .Where(t => t.AveragePricePerAdult < price)
                .ToListAsync(cancellationToken);
        }
        
        #endregion


    }
}
