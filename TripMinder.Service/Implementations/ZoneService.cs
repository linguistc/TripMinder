using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Service.Contracts;

namespace TripMinder.Service.Implementations;

public class ZoneService : IZoneService
{
    #region Fields
    private readonly IZoneRepository zoneRepository;
    #endregion
    
    #region Constructors
    public ZoneService(IZoneRepository zoneRepository)
    {
        this.zoneRepository = zoneRepository;
    }
    #endregion

    #region Methods

    public async Task<List<Zone>> GetZonesListAsync()
    {
        return await this.zoneRepository.GetTableNoTracking().Include(z => z.Governorate).ToListAsync();
    }

    public async Task<Zone> GetZoneByIdAsync(int id)
    {
        var zone = await this.zoneRepository.GetByIdAsync(id);
        return zone;
    }

    public async Task<bool> IsZoneIdExist(int id)
    {
        return await this.zoneRepository.GetTableNoTracking().AnyAsync(z => z.Id == id);
    }
    
    public async Task<string> CreateAsync(Zone zone)
    {
        await this.zoneRepository.CreateAsync(zone);
        return "Created";
    }

    public async Task<string> UpdateAsync(Zone zone)
    {
        await this.zoneRepository.UpdateAsync(zone);
        return "Updated";
    }

    public async Task<string> DeleteAsync(Zone zone)
    {
        var trans = this.zoneRepository.BeginTransaction();

        try
        {
            await this.zoneRepository.DeleteAsync(zone);
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