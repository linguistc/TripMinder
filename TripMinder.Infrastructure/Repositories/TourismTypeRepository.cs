using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class TourismTypeRepository : RepositoryAsync<TourismType>, ITourismTypeRepository
{
    #region Fields
    private readonly DbSet<TourismType> tourismTypes;
    #endregion

    #region Constructors    
    public TourismTypeRepository(AppDBContext dbContext) : base(dbContext)
    {
        tourismTypes = dbContext.Set<TourismType>();
    }
    #endregion

    #region Methods

    #endregion
}