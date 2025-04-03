using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IAccomodationClassService
{
    public Task<List<AccomodationClass>> GetAccomodationClassesListAsync();
    public Task<AccomodationClass> GetAccomodationClassByIdAsync(int id);
    public Task<bool> IsAccomodationClassIdExist(int id);
    public Task<string> CreateAsync(AccomodationClass accomodationClass);
    public Task<string> UpdateAsync(AccomodationClass accomodationClass);
    public Task<string> DeleteAsync(AccomodationClass accomodationClass);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}