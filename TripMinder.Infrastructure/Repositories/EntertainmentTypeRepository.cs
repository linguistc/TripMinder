using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class EntertainmentTypeRepository : RepositoryAsync<EntertainmentType>, IEntertainmentTypeRepository
{
    #region Fields
    private readonly DbSet<EntertainmentType> entertainmentTypes;
    #endregion

    #region Constructors    
    public EntertainmentTypeRepository(AppDBContext dbContext) : base(dbContext)
    {
        entertainmentTypes = dbContext.Set<EntertainmentType>();
    }
    #endregion

    #region Methods

    #endregion
}