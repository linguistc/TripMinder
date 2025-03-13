using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IFoodCategoryService
{
    public Task<List<FoodCategory>> GetFoodCategoriesListAsync();
    public Task<FoodCategory> GetFoodCategoryByIdAsync(int id);
    public Task<bool> IsFoodCategoryIdExist(int id);
    public Task<string> CreateAsync(FoodCategory foodCategory);
    public Task<string> UpdateAsync(FoodCategory foodCategory);
    public Task<string> DeleteAsync(FoodCategory foodCategory);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
}