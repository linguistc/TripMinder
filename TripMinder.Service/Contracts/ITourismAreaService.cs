using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface ITourismAreaService
    {
        public Task<List<TourismArea>> GetTourismAreasListAsync();
        public Task<List<TourismArea>> GetTourismAreasListByZoneIdAsync(int zoneId, CancellationToken cancellationToken = default);
        public Task<List<TourismArea>> GetTourismAreasListByGovernorateIdAsync(int governorateId, CancellationToken cancellationToken = default);
        public Task<TourismArea> GetTourismAreaByIdWithIncludeAsync(int id);
        public Task<TourismArea> GetTourismAreaByIdAsync(int id);
        public Task<string> CreateAsync(TourismArea newTourismArea);
        public Task<string> UpdateAsync(TourismArea tourismArea);
        public Task<string> DeleteAsync(TourismArea tourismArea);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);

    }
}
