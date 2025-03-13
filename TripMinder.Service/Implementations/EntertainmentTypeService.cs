using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class EntertainmentTypeService : IEntertainmentTypeService
{
    #region Fields

    private readonly IEntertainmentTypeRepository entertainmentTypeRepository;

    #endregion

    #region Constructors

    public EntertainmentTypeService(IEntertainmentTypeRepository entertainmentTypeRepository)
    {
        this.entertainmentTypeRepository = entertainmentTypeRepository;
    }

    #endregion

    #region Methods

    public async Task<List<EntertainmentType>> GetEntertainmentTypesListAsync()
    {
        return await this.entertainmentTypeRepository.GetTableNoTracking().ToListAsync();
    }

    public async Task<EntertainmentType> GetEntertainmentTypeByIdAsync(int id)
    {
        return await this.entertainmentTypeRepository.GetByIdAsync(id);
    }

    public async Task<bool> IsEntertainmentTypeIdExist(int id)
    {
        return await this.entertainmentTypeRepository.GetTableNoTracking().AnyAsync(a => a.Id == id);
    }

    public async Task<string> CreateAsync(EntertainmentType entertainmentType)
    {
        await this.entertainmentTypeRepository.CreateAsync(entertainmentType);
        return "Created";
    }

    public async Task<string> UpdateAsync(EntertainmentType entertainmentType)
    {
        await this.entertainmentTypeRepository.UpdateAsync(entertainmentType);
        return "Updated";
    }

    public async Task<string> DeleteAsync(EntertainmentType entertainmentType)
    {
        var trans = this.entertainmentTypeRepository.BeginTransaction();

        try
        {
            await this.entertainmentTypeRepository.DeleteAsync(entertainmentType);
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