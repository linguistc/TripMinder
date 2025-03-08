using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts
{
    public interface ITourismAreaService
    {
        public Task<List<TourismArea>> GetAllTourismAreasAsync();
        public Task<TourismArea> GetTourismAreaByIdWithIncludeAsync(int id);
        public Task<TourismArea> GetTourismAreaByIdAsync(int id);
        public Task<string> CreateAsync(TourismArea newTourismArea);

        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
        public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
        public Task<string> UpdateAsync(TourismArea tourismArea);
        public Task<string> DeleteAsync(TourismArea tourismArea);

    }
}
