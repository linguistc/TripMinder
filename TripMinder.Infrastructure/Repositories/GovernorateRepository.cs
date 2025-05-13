using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class GovernorateRepository : RepositoryAsync<Governorate>, IGovernorateRepository
{
    #region Fields

    private readonly DbSet<Governorate> governorates;

    #endregion

    #region Constructors

    public GovernorateRepository(AppDBContext dbContext) : base(dbContext)
    {
        governorates = dbContext.Set<Governorate>();
    }

    #endregion

    #region Methods

    #endregion
}