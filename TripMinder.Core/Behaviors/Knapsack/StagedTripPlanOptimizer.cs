using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class StagedTripPlanOptimizer : IStagedTripPlanOptimizer
{
    public async Task<List<Item>> OptimizeStagedAsync(
        List<Item> items,
        List<string> orderedInterests,
        int budget,
        UserDefinedKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        // Pre-check: Ensure budget is sufficient
        var minPrice = items.Any() ? items.Min(i => i.AveragePricePerAdult) : double.MaxValue;
        if (budget < minPrice)
        {
            Console.WriteLine($"Stopping: Budget {budget} < cheapest item price {minPrice}");
            return new List<Item>();
        }

        var phaseOrder = orderedInterests.Select(GetItemType).Distinct().ToList();
        var maxPerType = new Dictionary<ItemType, int>
        {
            [ItemType.Restaurant] = constraints.MaxRestaurants,
            [ItemType.Accommodation] = constraints.MaxAccommodations,
            [ItemType.Entertainment] = constraints.MaxEntertainments,
            [ItemType.TourismArea] = constraints.MaxTourismAreas
        };
        var dpItems = items.Select(i => new DpItem(i)).ToList();

        var states = new List<KnapsackState> { new KnapsackState { RemainingBudget = budget } };
        var bestState = states[0];
        int phase = 0;
        int unchangedPhases = 0;

        while (unchangedPhases < phaseOrder.Count && states.Any(s => s.RemainingBudget >= minPrice))
        {
            var currentType = phaseOrder[phase % phaseOrder.Count];
            int categoryLimit = (phase / phaseOrder.Count) + 1;
            if (bestState.CategoryCounts.GetValueOrDefault(currentType, 0) >= maxPerType[currentType])
            {
                unchangedPhases++;
                phase++;
                continue;
            }

            var newStates = new List<KnapsackState>();
            foreach (var state in states.ToList())
            {
                UpdateStatesForNewItem(state, dpItems, currentType, categoryLimit, phaseOrder.Count, budget, newStates);
                if (newStates.Any())
                {
                    states.AddRange(newStates);
                    var currentBest = states.OrderByDescending(s => s.TotalProfit).First();
                    if (currentBest.TotalProfit > bestState.TotalProfit)
                        bestState = currentBest;
                    unchangedPhases = 0;
                    Console.WriteLine($"Phase {phase + 1}: Added item for {currentType}, Best Profit={bestState.TotalProfit}");
                }
                else
                {
                    unchangedPhases++;
                    maxPerType[currentType] = state.CategoryCounts.GetValueOrDefault(currentType, 0);
                    Console.WriteLine($"Phase {phase + 1}: Failed to add for {currentType}, Fixed Max={maxPerType[currentType]}");
                }
            }

            states = states.OrderByDescending(s => s.TotalProfit).Take(100).ToList(); // Prune to top 100 states
            phase++;
        }

        var selectedItems = bestState.SelectedItems.Select(i => items.First(x => x.GlobalId == i.GlobalId && x.PlaceType == i.PlaceType)).ToList();
        Console.WriteLine($"Optimization Complete: Selected {selectedItems.Count} items, Total Profit={bestState.TotalProfit}, Total Cost={budget - bestState.RemainingBudget}");
        return selectedItems;
    }

    private void UpdateStatesForNewItem(KnapsackState state, List<DpItem> items, ItemType category, int categoryLimit, int totalCategories, int totalBudget, List<KnapsackState> newStates)
    {
        foreach (var item in items.Where(i => i.PlaceType == category))
        {
            if (state.RemainingBudget < item.Weight) continue;
            if (state.CategoryCounts.GetValueOrDefault(item.PlaceType, 0) >= categoryLimit) continue;
            if (state.UsedCategories.Contains(item.PlaceType)) continue;

            var newState = state.Clone();
            newState.TotalProfit += item.Profit;
            newState.RemainingBudget -= item.Weight;
            newState.SelectedItems.Add(item);

            if (!newState.CategoryCounts.ContainsKey(item.PlaceType))
                newState.CategoryCounts[item.PlaceType] = 0;
            newState.CategoryCounts[item.PlaceType]++;
            newState.UsedCategories.Add(item.PlaceType);

            if (newState.UsedCategories.Count == totalCategories)
            {
                newState.UsedCategories.Clear();
                newState.Phase++;
            }

            newStates.Add(newState);
        }
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