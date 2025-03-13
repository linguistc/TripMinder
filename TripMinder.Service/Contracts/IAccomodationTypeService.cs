using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IAccomodationTypeService
{
    public Task<List<AccomodationType>> GetAccomodationTypesListAsync();
    public Task<AccomodationType> GetAccomodationTypeByIdAsync(int id);
    public Task<bool> IsAccomodationTypeIdExist(int id);
    public Task<string> CreateAsync(AccomodationType accomodationType);
    public Task<string> UpdateAsync(AccomodationType accomodationType);
    public Task<string> DeleteAsync(AccomodationType accomodationType);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}