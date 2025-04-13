using MediatR;

namespace TripMinder.Core.Behaviors.Knapsack;

public interface IItemFetcher
{
    Task<List<Item>> FetchItems(int governorateId, int? zoneId, (int a, int f, int e, int t) priorities);
}