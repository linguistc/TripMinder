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
        // Convert items to DpItems
        var dpItems = items.Select(i => new DpItem(i)).ToList();

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
                if (currentMax[type] >= maxPerType[type])
                    continue;

                var phaseConstraints = new UserDefinedKnapsackConstraints(
                    currentMax.GetValueOrDefault(ItemType.Restaurant) + (type == ItemType.Restaurant ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.Accommodation) + (type == ItemType.Accommodation ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.Entertainment) + (type == ItemType.Entertainment ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.TourismArea) + (type == ItemType.TourismArea ? 1 : 0)
                );

                var (profit, itemsSelected) = await Task.Run(() =>
                    _solver.GetMaxProfit(
                        budget,
                        items, // KnapsackSolver will handle conversion to DpItem
                        phaseConstraints,
                        priorities,
                        true
                    )
                );

                int countOfType = itemsSelected.Count(i => i.PlaceType == type);
                if (countOfType > currentMax[type])
                {
                    currentMax[type]++;
                    lastSuccessMax[type] = currentMax[type];
                    bestItems = itemsSelected;
                    changedInThisLoop = true;
                }
                else
                {
                    currentMax[type] = lastSuccessMax[type];
                }
            }

            if (!changedInThisLoop || phaseOrder.All(t => currentMax[t] >= maxPerType[t]))
                break;

            var minPrices = phaseOrder
                .Select(t =>
                    items.Where(i => i.PlaceType == t).Select(i => i.AveragePricePerAdult)
                        .DefaultIfEmpty(double.MaxValue).Min())
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

