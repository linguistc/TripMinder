using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class TourismAreaClassRepository : RepositoryAsync<TourismAreaClass>, ITourismAreaClassRepository
{
    #region Fields
    private readonly DbSet<TourismAreaClass> tourismAreaClasses;
    #endregion

    #region Constructors
    public TourismAreaClassRepository(AppDBContext dbContext) : base(dbContext)
    {
        tourismAreaClasses = dbContext.Set<TourismAreaClass>();
    }
    #endregion

    #region Methods

    #endregion
}