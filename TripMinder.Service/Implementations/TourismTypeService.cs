using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class TourismTypeService : ITourismTypeService
{
    #region Fields

    private readonly ITourismTypeRepository tourismTypeRepository;

    #endregion

    #region Constructors

    public TourismTypeService(ITourismTypeRepository tourismTypeRepository)
    {
        this.tourismTypeRepository = tourismTypeRepository;
    }

    #endregion

    #region Methods
    public async Task<List<TourismType>> GetTourismTypesListAsync()
    {
        return await this.tourismTypeRepository.GetTableNoTracking().ToListAsync();
    }
    
    public async Task<TourismType> GetTourismTypeByIdAsync(int id)
    {
        return await this.tourismTypeRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsTourismTypeIdExist(int id)
    {
        return await this.tourismTypeRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }
    
    public async Task<string> CreateAsync(TourismType tourismType)
    {
        await this.tourismTypeRepository.CreateAsync(tourismType);    
        return "Created";
    }
    
    public async Task<string> UpdateAsync(TourismType tourismType)
    {
        await this.tourismTypeRepository.UpdateAsync(tourismType);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(TourismType tourismType)
    {
        var trans = this.tourismTypeRepository.BeginTransaction();    
        try
        {
            await this.tourismTypeRepository.DeleteAsync(tourismType);
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