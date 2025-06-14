using Microsoft.EntityFrameworkCore;
using TripMinder.Infrastructure.Bases;

namespace TripMinder.Infrastructure.Extentions;

public static class RepositoryAsyncExtensions
{
    public static async Task<double?> GetMinimumPriceAsync<T>(this IRepositoryAsync<T> repository, CancellationToken cancellationToken = default) where T : class
    {
        // التحقق إذا كان الكائن يدعم IPricedEntity
        if (typeof(IPricedEntity).IsAssignableFrom(typeof(T)))
        {
            var repo = repository as RepositoryAsync<T>; // التأكد من النوع
            if (repo != null)
            {
                return await repo.GetDbContext().Set<T>()
                    .Where(t => ((IPricedEntity)t).AveragePricePerAdult > 0)
                    .Select(t => ((IPricedEntity)t).AveragePricePerAdult)
                    .MinAsync();
            }
        }

        return null; // إرجاع null إذا لم يكن الكائن يدعم IPricedEntity
    }

    public static async Task<double?> GetMaximumPriceAsync<T>(this IRepositoryAsync<T> repository, CancellationToken cancellationToken = default) where T : class
    {
        if (typeof(IPricedEntity).IsAssignableFrom(typeof(T)))
        {
            var repo = repository as RepositoryAsync<T>;
            if (repo != null)
            {
                return await repo.GetDbContext().Set<T>()
                    .Where(t => ((IPricedEntity)t).AveragePricePerAdult > 0)
                    .Select(t => ((IPricedEntity)t).AveragePricePerAdult)
                    .MaxAsync();
            }
        }

        return null;
    }

    public static async Task<double?> GetAveragePriceAsync<T>(this IRepositoryAsync<T> repository, CancellationToken cancellationToken = default) where T : class
    {
        if (typeof(IPricedEntity).IsAssignableFrom(typeof(T)))
        {
            var repo = repository as RepositoryAsync<T>;
            if (repo != null)
            {
                return await repo.GetDbContext().Set<T>()
                    .Where(t => ((IPricedEntity)t).AveragePricePerAdult > 0)
                    .Select(t => ((IPricedEntity)t).AveragePricePerAdult)
                    .AverageAsync();
            }
        }

        return null;
    }
}
