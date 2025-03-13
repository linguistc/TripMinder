using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class EntertainmentClassService : IEntertainmentClassService
{
    #region Fields

    private readonly IEntertainmentClassRepository entertainmentClassRepository;

    #endregion

    #region Constructors

    public EntertainmentClassService(IEntertainmentClassRepository entertainmentClassRepository)
    {
        this.entertainmentClassRepository = entertainmentClassRepository;
    }

    #endregion

    #region Methods
    public async Task<List<EntertainmentClass>> GetEntertainmentClassesListAsync()
    {
        return await this.entertainmentClassRepository.GetTableNoTracking().ToListAsync();
    }
    
    public async Task<EntertainmentClass> GetEntertainmentClassByIdAsync(int id)
    {
        return await this.entertainmentClassRepository.GetByIdAsync(id);
    }
    
    public async Task<bool> IsEntertainmentClassIdExist(int id)
    {
        return await this.entertainmentClassRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }
    
    public async Task<string> CreateAsync(EntertainmentClass entertainmentClass)
    {
        await this.entertainmentClassRepository.CreateAsync(entertainmentClass);
        return "Created";
    }
    
    public async Task<string> UpdateAsync(EntertainmentClass entertainmentClass)
    {
        await this.entertainmentClassRepository.UpdateAsync(entertainmentClass);
        return "Updated";
    }
    
    public async Task<string> DeleteAsync(EntertainmentClass entertainmentClass)
    {
        var trans = this.entertainmentClassRepository.BeginTransaction();    

        try
        {
            await this.entertainmentClassRepository.DeleteAsync(entertainmentClass);
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