using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class AccomodationService : IAccomodationService
    {

        #region Fields
        private readonly IAccomodationRepository repository;

        #endregion

        #region Constructors
        public AccomodationService(IAccomodationRepository repository)
        {
            this.repository = repository;
        }
        #endregion

        #region Functions
        public Task<List<Accomodation>> GetAllAccomodationsAsync()
        {
            return this.repository.GetAllAccomodationsAsync();
        }

        public async Task<Accomodation> GetAccomodationByIdAsync(int id)
        {
            var accomodation = this.repository.GetTableNoTracking()
                                        .Include(a => a.Description)
                                        .Include(a => a.PlaceCategory)
                                        .Include(a => a.Class)
                                        .Include(a => a.Zone)
                                        .Include(a => a.Location)
                                        .FirstOrDefault(a => a.Id == id);

            return accomodation;
        }

        #endregion

    }
}
