using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class AccomodationTypeRepository : RepositoryAsync<AccomodationType>, IAccomodationTypeRepository
{
    #region Fields
    private readonly DbSet<AccomodationType> accomodationTypes;
    #endregion

    #region Constructors
    public AccomodationTypeRepository(AppDBContext dbContext) : base(dbContext)
    {
        accomodationTypes = dbContext.Set<AccomodationType>();
    }
    #endregion

    #region Methods

    #endregion
}