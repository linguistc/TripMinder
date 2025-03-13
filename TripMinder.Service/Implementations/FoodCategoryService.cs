using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class FoodCategoryService : IFoodCategoryService
{
    #region Fields

    private readonly IFoodCategoryRepository foodCategoryRepository;

    #endregion

    #region Constructors

    public FoodCategoryService(IFoodCategoryRepository foodCategoryRepository)
    {
        this.foodCategoryRepository = foodCategoryRepository;
    }

    #endregion

    #region Methods

    public async Task<List<FoodCategory>> GetFoodCategoriesListAsync()
    {
        return await this.foodCategoryRepository.GetTableNoTracking().ToListAsync();
    }

    public async Task<FoodCategory> GetFoodCategoryByIdAsync(int id)
    {
        return await this.foodCategoryRepository.GetByIdAsync(id);
    }

    public async Task<bool> IsFoodCategoryIdExist(int id)
    {
        return await this.foodCategoryRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }

    public async Task<string> CreateAsync(FoodCategory foodCategory)
    {
        await this.foodCategoryRepository.CreateAsync(foodCategory);
        return "Created";
    }

    public async Task<string> UpdateAsync(FoodCategory foodCategory)
    {
        await this.foodCategoryRepository.UpdateAsync(foodCategory);
        return "Updated";
    }

    public async Task<string> DeleteAsync(FoodCategory foodCategory)
    {
        var trans = this.foodCategoryRepository.BeginTransaction();

        try
        {
            await this.foodCategoryRepository.DeleteAsync(foodCategory);
            await trans.CommitAsync();
            return "Deleted";
        }
        catch
        {
            await trans.RollbackAsync();
            return "Failed";
        }
    }

    public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsNameArExist(string nameAr)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsNameEnExist(string nameEn)
    {
        throw new NotImplementedException();
    }

    #endregion
}