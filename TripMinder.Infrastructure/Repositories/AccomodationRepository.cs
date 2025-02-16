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
        public async Task<List<Accomodation>> GetAllAccomodationsAsync()
        {

            var result = await this.accomodations.Include(r => r.Description)
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
