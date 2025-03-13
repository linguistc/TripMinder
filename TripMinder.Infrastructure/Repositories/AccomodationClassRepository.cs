using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class AccomodationClassRepository : RepositoryAsync<AccomodationClass>, IAccomodationClassRepository
{
    #region Fields
    private readonly DbSet<AccomodationClass> accomodationClasses;
    #endregion

    #region Constructors
    public AccomodationClassRepository(AppDBContext dbContext) : base(dbContext)
    {
        accomodationClasses = dbContext.Set<AccomodationClass>();
    }
    #endregion

    #region Methods

    #endregion
}