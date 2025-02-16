using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IAccomodationService
    {
        public Task<List<Accomodation>> GetAllAccomodationsAsync();
        public Task<Accomodation> GetAccomodationByIdAsync(int id);
    }
}
