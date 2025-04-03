using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class ZoneRepository : RepositoryAsync<Zone>, IZoneRepository
{
    #region Fields
    private readonly DbSet<Zone> zones;
    #endregion

    #region Constructors
    public ZoneRepository(AppDBContext dbContext) : base(dbContext)
    {
        zones = dbContext.Set<Zone>();
    }
    #endregion

    #region Methods

    #endregion
}