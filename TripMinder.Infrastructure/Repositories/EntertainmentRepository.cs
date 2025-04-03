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
        public async Task<List<Entertainment>> GetAllEntertainmentsAsync()
        {

            var result = await this.entertainments.Include(r => r.EntertainmentType)
                                         .Include(r => r.Zone)
                                         .Include(r => r.Class)
                                         .Include(r => r.PlaceType)
                                         .ToListAsync();

            return result;
        }

        #endregion


    }
}
