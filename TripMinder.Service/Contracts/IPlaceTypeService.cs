using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IPlaceTypeService
{
    public Task<List<PlaceType>> GetPlaceTypesListAsync();
    public Task<PlaceType> GetPlaceTypeByIdAsync(int id);
    public Task<bool> IsPlaceTypeIdExist(int id);
    public Task<string> CreateAsync(PlaceType placeType);
    public Task<string> UpdateAsync(PlaceType placeType);
    public Task<string> DeleteAsync(PlaceType placeType);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}