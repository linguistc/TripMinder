using Microsoft.EntityFrameworkCore;
using TripMinder.Data.Entities;
using TripMinder.Infrastructure.Bases;
using TripMinder.Infrastructure.Contracts;
using TripMinder.Infrastructure.Data;

namespace TripMinder.Infrastructure.Repositories;

public class FoodCategoryRepository : RepositoryAsync<FoodCategory>, IFoodCategoryRepository
{
    #region Fields
    private readonly DbSet<FoodCategory> foodCategories;
    #endregion

    #region Constructors
    public FoodCategoryRepository(AppDBContext dbContext) : base(dbContext)
    {
        foodCategories = dbContext.Set<FoodCategory>();
    }
    #endregion

    #region Methods

    #endregion
}