using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class EntertainmentClassRepository : RepositoryAsync<EntertainmentClass>, IEntertainmentClassRepository
{
    #region Fields
    private readonly DbSet<EntertainmentClass> entertainmentClasses;
    #endregion

    #region Constructors
    public EntertainmentClassRepository(AppDBContext dbContext) : base(dbContext)
    {
        entertainmentClasses = dbContext.Set<EntertainmentClass>();
    }
    #endregion

    #region Methods

    #endregion
}