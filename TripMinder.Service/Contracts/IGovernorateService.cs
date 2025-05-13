using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IGovernorateService
{
    public Task<List<Governorate>> GetGovernoratesListAsync();
    public Task<Governorate> GetGovernorateByIdAsync(int id);
    public Task<bool> IsGovernorateIdExist(int id);
    public Task<string> CreateAsync(Governorate governorate);
    public Task<string> UpdateAsync(Governorate governorate);
    public Task<string> DeleteAsync(Governorate governorate);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}