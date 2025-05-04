using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

/// <summary>
/// Greedy staged optimizer: incrementally adds highest "profit" items per user priority, 
/// respecting per-type max constraints, phased expansion, and early stop.
/// </summary>
public class GreedyPhaseOptimizer : IGreedyPhaseOptimizer
{
    public async Task<List<Item>> OptimizePhasedAsync(
        List<Item> items,
        List<string> orderedInterests,
        int budget,
        UserDefinedKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        var phaseOrder = orderedInterests.Select(GetItemType).Distinct().ToList();
        var tracker = new CountTracker();
        var maxPerType = new Dictionary<ItemType, int>
        {
            [ItemType.Accommodation] = constraints.MaxAccommodations,
            [ItemType.Restaurant] = constraints.MaxRestaurants,
            [ItemType.Entertainment] = constraints.MaxEntertainments,
            [ItemType.TourismArea] = constraints.MaxTourismAreas
        };
        var fixedMax = phaseOrder.ToDictionary(t => t, t => 0);
        var selectedItems = new List<Item>();
        int remainingBudget = budget;

        var priorityWeights = new Dictionary<ItemType, float>();
        foreach (var type in phaseOrder)
        {
            int priority = type switch
            {
                ItemType.Accommodation => priorities?.a ?? 0,
                ItemType.Restaurant => priorities?.f ?? 0,
                ItemType.Entertainment => priorities?.e ?? 0,
                ItemType.TourismArea => priorities?.t ?? 0,
                _ => 0
            };
            priorityWeights[type] = priority > 0 ? (float)Math.Pow(0.8, priority - 1) : 0.1f;
        }

        var itemScores = items
            .Select(item => new
            {
                Item = item,
                AdjustedRatio = item.AveragePricePerAdult > 0
                    ? (item.Score * priorityWeights.GetValueOrDefault(item.PlaceType, 0.1f)) / item.AveragePricePerAdult
                    : 0.0
            })
            .OrderByDescending(x => x.AdjustedRatio)
            .ToList();

        int unchangedPhases = 0;
        while (unchangedPhases < phaseOrder.Count && remainingBudget > 0)
        {
            bool changedInThisPhase = false;

            foreach (var type in phaseOrder)
            {
                if (tracker.Exceeded(type, constraints) || tracker.GetCount(type) >= maxPerType[type])
                {
                    Console.WriteLine($"Skipping {type}: Max reached ({tracker.GetCount(type)}/{maxPerType[type]})");
                    continue;
                }

                var candidate = itemScores
                    .FirstOrDefault(x =>
                        x.Item.PlaceType == type &&
                        !selectedItems.Contains(x.Item) &&
                        x.Item.AveragePricePerAdult <= remainingBudget);

                if (candidate != null)
                {
                    selectedItems.Add(candidate.Item);
                    remainingBudget -= (int)candidate.Item.AveragePricePerAdult;
                    tracker.Increment(type);
                    fixedMax[type] = tracker.GetCount(type);
                    changedInThisPhase = true;
                    Console.WriteLine($"Phase Success: Added {type}, Item={candidate.Item.Name}, Score={candidate.Item.Score}, Cost={candidate.Item.AveragePricePerAdult}, RemainingBudget={remainingBudget}");
                }
                else
                {
                    fixedMax[type] = tracker.GetCount(type);
                    Console.WriteLine($"Phase Failure: No valid item for {type} at count {tracker.GetCount(type)}");
                }
            }

            unchangedPhases = changedInThisPhase ? 0 : unchangedPhases + 1;

            var minPrice = phaseOrder
                .Select(t => items
                    .Where(i => i.PlaceType == t && !selectedItems.Contains(i))
                    .Select(i => i.AveragePricePerAdult)
                    .DefaultIfEmpty(double.MaxValue)
                    .Min())
                .Min();
            if (remainingBudget < minPrice)
            {
                Console.WriteLine($"Stopping: Remaining budget {remainingBudget} < min price {minPrice}");
                break;
            }
        }

        Console.WriteLine($"Greedy Phased Optimization Complete: Selected {selectedItems.Count} items, Total Score={selectedItems.Sum(i => i.Score)}, Total Cost={selectedItems.Sum(i => i.AveragePricePerAdult)}");
        return selectedItems;
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