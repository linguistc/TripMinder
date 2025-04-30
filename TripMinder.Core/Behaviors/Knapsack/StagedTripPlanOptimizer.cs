
namespace TripMinder.Core.Behaviors.Knapsack;

using System.Collections.Generic;
using System.Threading.Tasks;

public class StagedTripPlanOptimizer : IStagedTripPlanOptimizer
{
    private readonly IKnapsackSolver _solver;

    public StagedTripPlanOptimizer(IKnapsackSolver solver)
    {
        _solver = solver;
    }

    public async Task<List<Item>> OptimizeStagedAsync(
        List<Item> items,
        List<string> orderedInterests,
        int budget,
        UserDefinedKnapsackConstraints originalConstraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        // Prepare types and constraints
        var phaseOrder = orderedInterests.Select(GetItemType).ToList();
        var maxPerType = new Dictionary<ItemType, int>
        {
            [ItemType.Accommodation] = originalConstraints.MaxAccommodations,
            [ItemType.Restaurant] = originalConstraints.MaxRestaurants,
            [ItemType.Entertainment] = originalConstraints.MaxEntertainments,
            [ItemType.TourismArea] = originalConstraints.MaxTourismAreas
        };
        var currentMax = phaseOrder.ToDictionary(t => t, t => 0);
        var lastSuccessMax = phaseOrder.ToDictionary(t => t, t => 0);
        var bestItems = new List<Item>();

        // Phased Expansion Loop
        while (true)
        {
            bool changedInThisLoop = false;

            foreach (var type in phaseOrder)
            {
                // Don't exceed user max
                if (currentMax[type] >= maxPerType[type])
                    continue;

                // Prepare constraints for this phase
                var phaseConstraints = new UserDefinedKnapsackConstraints(
                    currentMax.GetValueOrDefault(ItemType.Restaurant) + (type == ItemType.Restaurant ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.Accommodation) + (type == ItemType.Accommodation ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.Entertainment) + (type == ItemType.Entertainment ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.TourismArea) + (type == ItemType.TourismArea ? 1 : 0)
                );

                // Run knapsack for this phase (always with full budget)
                var (profit, itemsSelected) = await Task.Run(() =>
                    _solver.GetMaxProfit(
                        budget,
                        items,
                        phaseConstraints,
                        priorities,
                        true // requireExact = true
                    )
                );

                int countOfType = itemsSelected.Count(i => i.PlaceType == type);
                if (countOfType > currentMax[type])
                {
                    // Success: update max and bestItems
                    currentMax[type]++;
                    lastSuccessMax[type] = currentMax[type];
                    bestItems = itemsSelected;
                    changedInThisLoop = true;
                }
                else
                {
                    // Failed to add more: fix max at last successful
                    currentMax[type] = lastSuccessMax[type];
                }
            }

            // Early stop: if no type increased in this full loop, or all max reached
            if (!changedInThisLoop || phaseOrder.All(t => currentMax[t] >= maxPerType[t]))
                break;

            // Also, if budget is less than min price of any available item, stop
            var minPrices = phaseOrder
                .Select(t => items.Where(i => i.PlaceType == t).Select(i => i.AveragePricePerAdult).DefaultIfEmpty(double.MaxValue).Min())
                .ToList();
            if (budget < minPrices.Min())
                break;
        }

        return bestItems;
    }

    private ItemType GetItemType(string interest)
    {
        return interest?.Trim().ToLowerInvariant() switch
        {
            "accommodation" => ItemType.Accommodation,
            "restaurants" or "food" => ItemType.Restaurant,
            "entertainments" or "entertainment" => ItemType.Entertainment,
            "tourismareas" or "tourism" => ItemType.TourismArea,
            _ => throw new ArgumentException($"Unknown interest: {interest}")
        };
    }
}
