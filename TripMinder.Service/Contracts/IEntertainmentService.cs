using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IEntertainmentService
    {
        public Task<List<Entertainment>> GetAllEntertainmentsAsync();
        public Task<Entertainment> GetEntertainmentByIdAsync(int id);
    }}
