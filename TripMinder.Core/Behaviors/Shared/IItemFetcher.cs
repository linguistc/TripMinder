using MediatR;

namespace TripMinder.Core.Behaviors.Shared;

public interface IItemFetcher
{
    Task<List<Item>> FetchItems(int governorateId, int? zoneId, (int a, int f, int e, int t) priorities, double dailyBudgetPerAdult);
}