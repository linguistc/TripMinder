using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class TourismAreaClassService : ITourismAreaClassService
{
    #region Fields

    private readonly ITourismAreaClassRepository tourismAreaClassRepository;

    #endregion

    #region Constructors

    public TourismAreaClassService(ITourismAreaClassRepository tourismAreaClassRepository)
    {
        this.tourismAreaClassRepository = tourismAreaClassRepository;
    }

    #endregion   

    #region Methods
    public async Task<List<TourismAreaClass>> GetTourismAreaClassesListAsync()
    {
        return await this.tourismAreaClassRepository.GetTableNoTracking().ToListAsync();
    }
    
    public async Task<TourismAreaClass> GetTourismAreaClassByIdAsync(int id)
    {
        return await this.tourismAreaClassRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsTourismAreaClassIdExist(int id)
    {
        return await this.tourismAreaClassRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }
    
    public async Task<string> CreateAsync(TourismAreaClass tourismAreaClass)
    {
        await this.tourismAreaClassRepository.CreateAsync(tourismAreaClass);
        return "Created";
    }
    
    public async Task<string> UpdateAsync(TourismAreaClass tourismAreaClass)
    {
        await this.tourismAreaClassRepository.UpdateAsync(tourismAreaClass);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(TourismAreaClass tourismAreaClass)
    {
        var trans = this.tourismAreaClassRepository.BeginTransaction();

        try
        {
            await this.tourismAreaClassRepository.DeleteAsync(tourismAreaClass);
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