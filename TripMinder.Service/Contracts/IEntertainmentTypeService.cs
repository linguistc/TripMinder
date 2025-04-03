using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IEntertainmentTypeService
{
    public Task<List<EntertainmentType>> GetEntertainmentTypesListAsync();
    public Task<EntertainmentType> GetEntertainmentTypeByIdAsync(int id);
    public Task<bool> IsEntertainmentTypeIdExist(int id);
    public Task<string> CreateAsync(EntertainmentType entertainmentType);
    public Task<string> UpdateAsync(EntertainmentType entertainmentType);
    public Task<string> DeleteAsync(EntertainmentType entertainmentType);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}