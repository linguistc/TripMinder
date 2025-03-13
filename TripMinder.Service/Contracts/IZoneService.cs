using TripMinder.Data.Entities;

namespace TripMinder.Service.Contracts;

public interface IZoneService
{
    public Task<List<Zone>> GetZonesListAsync();
    public Task<Zone> GetZoneByIdAsync(int id);
    public Task<bool> IsZoneIdExist(int id);
    public Task<string> CreateAsync(Zone zone);
    public Task<string> UpdateAsync(Zone zone);
    public Task<string> DeleteAsync(Zone zone);
    public Task<bool> IsNameArExistExcludeSelf(string nameAr, int id);
    public Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id);
    public Task<bool> IsNameArExist(string nameAr);
    public Task<bool> IsNameEnExist(string nameEn);
    // public Task<Zone> GetZoneByIdWithIncludeAsync(int id);
}