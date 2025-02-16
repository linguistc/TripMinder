using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IAccomodationRepository : IRepositoryAsync<Accomodation>
    {
        public Task<List<Accomodation>> GetAllAccomodationsAsync();

    }
}
