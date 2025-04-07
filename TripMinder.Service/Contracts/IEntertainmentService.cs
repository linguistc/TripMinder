using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IEntertainmentService
    {
        public Task<List<Entertainment>> GetEntertainmentsListAsync();
        public Task<List<Entertainment>> GetEntertainmentsListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<Entertainment>> GetEntertainmentsListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        public Task<Entertainment> GetEntertainmentByIdWithIncludeAsync(int id);
        public Task<Entertainment> GetEntertainmentByIdAsync(int id);
        public Task<string> CreateAsync(Entertainment newEntertainment);
        public Task<string> UpdateAsync(Entertainment entertainment);
        public Task<string> DeleteAsync(Entertainment entertainment);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);

    }}
