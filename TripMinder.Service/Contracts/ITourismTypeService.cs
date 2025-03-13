using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface ITourismTypeService
{
    public Task<List<TourismType>> GetTourismTypesListAsync();
    public Task<TourismType> GetTourismTypeByIdAsync(int id);
    public Task<bool> IsTourismTypeIdExist(int id);
    public Task<string> CreateAsync(TourismType tourismType);
    public Task<string> UpdateAsync(TourismType tourismType);
    public Task<string> DeleteAsync(TourismType tourismType);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}