using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface ITourismAreaClassService
{
    public Task<List<TourismAreaClass>> GetTourismAreaClassesListAsync();
    public Task<TourismAreaClass> GetTourismAreaClassByIdAsync(int id);
    public Task<bool> IsTourismAreaClassIdExist(int id);
    public Task<string> CreateAsync(TourismAreaClass tourismAreaClass);
    public Task<string> UpdateAsync(TourismAreaClass tourismAreaClass);
    public Task<string> DeleteAsync(TourismAreaClass tourismAreaClass);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}