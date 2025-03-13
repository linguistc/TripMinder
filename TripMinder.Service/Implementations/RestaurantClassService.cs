using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class RestaurantClassService : IRestaurantClassService
{
    #region Fields

    private readonly IRestaurantClassRepository restaurantClassRepository;

    #endregion

    #region Constructors

    public RestaurantClassService(IRestaurantClassRepository restaurantClassRepository)
    {
        this.restaurantClassRepository = restaurantClassRepository;
    }

    #endregion

    #region Methods
    public async Task<List<RestaurantClass>> GetRestaurantClassesListAsync()
    {
        return await this.restaurantClassRepository.GetTableNoTracking().ToListAsync();
    }
    
    public async Task<RestaurantClass> GetRestaurantClassByIdAsync(int id)
    {
        return await this.restaurantClassRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsRestaurantClassIdExist(int id)
    {
        return await this.restaurantClassRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }
    
    public async Task<string> CreateAsync(RestaurantClass restaurantClass)
    {
        await this.restaurantClassRepository.CreateAsync(restaurantClass);
        return "Created";
    }
    
    public async Task<string> UpdateAsync(RestaurantClass restaurantClass)
    {
        await this.restaurantClassRepository.UpdateAsync(restaurantClass);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(RestaurantClass restaurantClass)
    {
        var trans = this.restaurantClassRepository.BeginTransaction();

        try
        {
            await this.restaurantClassRepository.DeleteAsync(restaurantClass);
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