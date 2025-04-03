using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class PlaceTypeService : IPlaceTypeService
{
    #region Fields

    private readonly IPlaceTypeRepository placeTypeRepository;

    #endregion

    #region Constructors

    public PlaceTypeService(IPlaceTypeRepository placeTypeRepository)
    {
        this.placeTypeRepository = placeTypeRepository;
    }

    #endregion

    #region Methods
    public async Task<List<PlaceType>> GetPlaceTypesListAsync()
    {
        return await this.placeTypeRepository.GetTableNoTracking().ToListAsync();
    }
    
    public async Task<PlaceType> GetPlaceTypeByIdAsync(int id)
    {
        return await this.placeTypeRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsPlaceTypeIdExist(int id)
    {
        return await this.placeTypeRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }
    
    public async Task<string> CreateAsync(PlaceType placeType)
    {
        await this.placeTypeRepository.CreateAsync(placeType);
        return "Created";
    }
    
    public async Task<string> UpdateAsync(PlaceType placeType)
    {
        await this.placeTypeRepository.UpdateAsync(placeType);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(PlaceType placeType)
    {
        var trans = this.placeTypeRepository.BeginTransaction();

        try
        {
            await this.placeTypeRepository.DeleteAsync(placeType);
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