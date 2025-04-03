using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class AccomodationClassService : IAccomodationClassService
{
    #region Fields
    private readonly IAccomodationClassRepository accomodationClassRepository;
    #endregion
    
    #region Constructors
    public AccomodationClassService(IAccomodationClassRepository accomodationClassRepository)
    {
        this.accomodationClassRepository = accomodationClassRepository;
    }
    #endregion

    #region Methods
    public async Task<List<AccomodationClass>> GetAccomodationClassesListAsync()
    {
        return await this.accomodationClassRepository.GetTableNoTracking().ToListAsync();
    }

    public async Task<AccomodationClass> GetAccomodationClassByIdAsync(int id)
    {
        return await this.accomodationClassRepository.GetByIdAsync(id);
    }

    public async Task<bool> IsAccomodationClassIdExist(int id)
    {
        return await this.accomodationClassRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }

    public async Task<string> CreateAsync(AccomodationClass accomodationClass)
    {
        await this.accomodationClassRepository.CreateAsync(accomodationClass);
        return "Created";
    }

    public async Task<string> UpdateAsync(AccomodationClass accomodationClass)
    {
        await this.accomodationClassRepository.UpdateAsync(accomodationClass);
        return "Updated";
    }

    public async Task<string> DeleteAsync(AccomodationClass accomodationClass)
    {
        var trans = this.accomodationClassRepository.BeginTransaction();

        try
        {
            await this.accomodationClassRepository.DeleteAsync(accomodationClass);
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