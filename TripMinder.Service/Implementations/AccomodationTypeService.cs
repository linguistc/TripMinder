using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class AccomodationTypeService : IAccomodationTypeService
{
    #region Fields

    private readonly IAccomodationTypeRepository accomodationTypeRepository;

    #endregion

    #region Constructors

    public AccomodationTypeService(IAccomodationTypeRepository accomodationTypeRepository)
    {
        this.accomodationTypeRepository = accomodationTypeRepository;
    }

    #endregion

    #region Methods
    public async Task<List<AccomodationType>> GetAccomodationTypesListAsync()
    {
        return await this.accomodationTypeRepository.GetTableNoTracking().ToListAsync();
    }
    
    public async Task<AccomodationType> GetAccomodationTypeByIdAsync(int id)
    {
        return await this.accomodationTypeRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsAccomodationTypeIdExist(int id)
    {
        return await this.accomodationTypeRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }
    
    public async Task<string> CreateAsync(AccomodationType accomodationType)
    {
        await this.accomodationTypeRepository.CreateAsync(accomodationType);
        return "Created";
    }
    
    public async Task<string> UpdateAsync(AccomodationType accomodationType)
    {
        await this.accomodationTypeRepository.UpdateAsync(accomodationType);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(AccomodationType accomodationType)
    {
        var trans = this.accomodationTypeRepository.BeginTransaction();

        try
        {
            await this.accomodationTypeRepository.DeleteAsync(accomodationType);
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