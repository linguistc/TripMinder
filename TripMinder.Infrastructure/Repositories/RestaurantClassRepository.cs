using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class RestaurantClassRepository : RepositoryAsync<RestaurantClass>, IRestaurantClassRepository
{
    #region Fields
    private readonly DbSet<RestaurantClass> restaurantClasses;
    #endregion

    #region Constructors
    public RestaurantClassRepository(AppDBContext dbContext) : base(dbContext)
    {
        restaurantClasses = dbContext.Set<RestaurantClass>();
    }
    #endregion
    
    #region Methods

    #endregion
}