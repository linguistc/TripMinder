using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories
{
    public class AccomodationRepository : RepositoryAsync<Accomodation>, IAccomodationRepository
    {

        #region Fields
        private readonly DbSet<Accomodation> accomodations;

        #endregion

        #region Constructors
        public AccomodationRepository(AppDBContext dbContext) : base(dbContext)
        {
            this.accomodations = dbContext.Set<Accomodation>();
        }

        #endregion

        #region Functions
        public async Task<List<Accomodation>> GetAccomodationsListAsync()
        {

            var result = await this.accomodations.Include(r => r.AccomodationType)
                                         .Include(r => r.Zone).AsNoTracking()
                                         .Include(r => r.Zone.Governorate).AsNoTracking()
                                         .Include(r => r.Class)
                                         .Include(r => r.PlaceType)
                                         .ToListAsync();

            return result;
        }

        public async Task<List<Accomodation>> GetAccomodationsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                    .Include(r => r.AccomodationType)
                    .Include(r => r.Zone).AsNoTracking()
                    .Include(r => r.Zone.Governorate).AsNoTracking()
                    .Include(r => r.Class)
                    .Include(r => r.PlaceType)
                    .Where(r => r.ZoneId == zoneId)
                    .ToListAsync(cancellationToken); 
        }


        public async Task<List<Accomodation>> GetAccomodationsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                    .Include(r => r.AccomodationType)
                    .Include(r => r.Zone).AsNoTracking()
                    .Include(r => r.Zone.Governorate).AsNoTracking()
                    .Include(r => r.Class)
                    .Include(r => r.PlaceType)
                    .Where(r => r.Zone.GovernorateId == governorateId)
                    .ToListAsync(cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationListByClassIdAsync(int classId, CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(r => r.ClassId == classId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Accomodation>> GetAccomodationListByTypeIdAsync(int TypeId, CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.AccomodationTypeId == TypeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Accomodation>> GetAccomodationListByNumOfBedsAsync(short numOfBeds,
            CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.NumOfBeds == numOfBeds)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Accomodation>> GetAccomodationListByNumOfNembersAsync(short numOfMembers,
            CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.NumOfMembers == numOfMembers)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Accomodation>> GetAccomodationListLessThanPriceAsync(double price,
            CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.AveragePricePerAdult < price)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Accomodation>> GetAccomodationListMoreThanPriceAsync(double price,
            CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.AveragePricePerAdult > price)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Accomodation>> GetAccomodationListByRatingAsync(double rating,
            CancellationToken cancellationToken = default)
        {
            return await this.accomodations
                .Include(r => r.AccomodationType)
                .Include(r => r.Zone).AsNoTracking()
                .Include(r => r.Zone.Governorate).AsNoTracking()
                .Include(r => r.Class)
                .Include(r => r.PlaceType)
                .Where(a => a.Rating > rating)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
