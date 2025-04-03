using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class PlaceTypeRepository : RepositoryAsync<PlaceType>, IPlaceTypeRepository
{
    #region Fields
    private readonly DbSet<PlaceType> placeTypes;
    #endregion

    #region Constructors
    public PlaceTypeRepository(AppDBContext dbContext) : base(dbContext)
    {
        placeTypes = dbContext.Set<PlaceType>();
    }
    #endregion

    #region Methods

    #endregion
}