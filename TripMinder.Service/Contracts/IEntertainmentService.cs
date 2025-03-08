using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface IEntertainmentService
    {
        public Task<List<Entertainment>> GetAllEntertainmentsAsync();
        public Task<Entertainment> GetEntertainmentByIdWithIncludeAsync(int id);
        public Task<Entertainment> GetEntertainmentByIdAsync(int id);
        public Task<string> CreateAsync(Entertainment newEntertainment);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
        public Task<string> UpdateAsync(Entertainment entertainment);
        public Task<string> DeleteAsync(Entertainment entertainment);

    }}
