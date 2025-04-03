using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IEntertainmentClassService
{
    public Task<List<EntertainmentClass>> GetEntertainmentClassesListAsync();
    public Task<EntertainmentClass> GetEntertainmentClassByIdAsync(int id);
    public Task<bool> IsEntertainmentClassIdExist(int id);
    public Task<string> CreateAsync(EntertainmentClass entertainmentClass);
    public Task<string> UpdateAsync(EntertainmentClass entertainmentClass);
    public Task<string> DeleteAsync(EntertainmentClass entertainmentClass);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}