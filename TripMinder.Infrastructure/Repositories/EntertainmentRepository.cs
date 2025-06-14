using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories
{
    public class EntertainmentRepository : RepositoryAsync<Entertainment>, IEntertainmentRepository
    {

        #region Fields
        private readonly DbSet<Entertainment> entertainments;

        #endregion

        #region Constructors
        public EntertainmentRepository(AppDBContext dbContext) : base(dbContext)
        {
            this.entertainments = dbContext.Set<Entertainment>();
        }

        #endregion

        #region Functions
        public async Task<List<Entertainment>> GetEntertainmentsListAsync()
        {

            var result = await this.entertainments.Include(r => r.EntertainmentType)
                                         .Include(r => r.Zone).AsNoTracking()
                                         .Include(r => r.Zone.Governorate).AsNoTracking()
                                         .Include(r => r.Class)
                                         .Include(r => r.PlaceType)
                                         .ToListAsync();

            return result;
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                                         .Include(r => r.Zone).AsNoTracking()
                                         .Include(r => r.Zone.Governorate).AsNoTracking()
                                         .Include(r => r.Class)
                                         .Include(r => r.PlaceType)
                                         .Where(r => r.ZoneId == zoneId)
                                         .ToListAsync(cancellationToken);
        }

        public async Task<List<Entertainment>> GetEntertainmentsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(r => r.Zone.GovernorateId == governorateId)
                .ToListAsync(cancellationToken);
        }

        
        public async Task<List<Entertainment>> GetEntertainmentsListByClassIdAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(r => r.ClassId == classId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Entertainment>> GetEntertainmentsListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.EntertainmentTypeId == TypeId)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Entertainment>> GetEntertainmentsListMoreThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.AveragePricePerAdult > price)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Entertainment>> GetEntertainmentsListLessThanPriceAsync(double price, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.AveragePricePerAdult < price)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Entertainment>> GetEntertainmentsListByRatingAsync(double rating, CancellationToken cancellationToken = default)
        {
            return await this.entertainments
                .Include(r => r.EntertainmentType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.Rating >= rating)
                .ToListAsync(cancellationToken);
        }
        
        
        #endregion


    }
}
