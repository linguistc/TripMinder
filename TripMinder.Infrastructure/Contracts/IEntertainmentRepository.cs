using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Contracts
{
    public interface IEntertainmentRepository : IRepositoryAsync<Entertainment>
    {
        public Task<List<Entertainment>> GetAllEntertainmentsAsync();

    }
}
