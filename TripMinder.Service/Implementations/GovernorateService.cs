using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class GovernorateService : IGovernorateService
{
    #region Fields
    private readonly IGovernorateRepository governorateRepository;
    #endregion
    
    #region Constructors
    public GovernorateService(IGovernorateRepository governorateRepository)
    {
        this.governorateRepository = governorateRepository;
    }
    #endregion
    
    #region Methods
    public async Task<List<Governorate>> GetGovernoratesListAsync()
    {
        return await this.governorateRepository.GetTableNoTracking().ToListAsync();
    }

    public async Task<Governorate> GetGovernorateByIdAsync(int id)
    {
        return await this.governorateRepository.GetByIdAsync(id);
    }

    public async Task<bool> IsGovernorateIdExist(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> CreateAsync(Governorate governorate)
    {
        await this.governorateRepository.CreateAsync(governorate);
        return "Created";
    }
    
    public async Task<string> UpdateAsync(Governorate governorate)
    {
        await this.governorateRepository.UpdateAsync(governorate);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(Governorate governorate)
    {
        var trans = this.governorateRepository.BeginTransaction();

        try
        {
            await this.governorateRepository.DeleteAsync(governorate);
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