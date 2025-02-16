using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations
{
    public class EntertainmentService : IEntertainmentService
    {
        #region Fields
        private readonly IEntertainmentRepository repository;

        #endregion

        #region Constructors
        public EntertainmentService(IEntertainmentRepository repository)
        {
            this.repository = repository;
        }

        #endregion

        #region Functions
        public async Task<List<Entertainment>> GetAllEntertainmentsAsync()
        {
            return await this.repository.GetAllEntertainmentsAsync();
        }

        public async Task<Entertainment> GetEntertainmentByIdAsync(int id)
        {
            var entertainment = this.repository.GetTableNoTracking()
                                            .Include(e => e.Description)
                                            .Include(e => e.PlaceCategory)
                                            .Include(e => e.Class)
                                            .Include(e => e.Zone)
                                            .Include(e => e.Location)
                                            .Where(e => e.Id.Equals(id))
                                            .FirstOrDefault();

            return entertainment;
        }

        #endregion
    }
}
