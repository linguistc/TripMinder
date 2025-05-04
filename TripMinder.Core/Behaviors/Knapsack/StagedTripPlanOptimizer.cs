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

        // Calculate priority weights
        var priorityWeights = new Dictionary<ItemType, double>();
        for (int i = 0; i < phaseOrder.Count; i++)
        {
            priorityWeights[phaseOrder[i]] = 1.0 + (phaseOrder.Count - i) * 0.5; // e.g., 2.5, 2.0, 1.5, 1.0
        }

        var bestSolution = new KnapsackState { RemainingBudget = budget };
        int phase = 0;
        int unchangedPhases = 0;
        var currentCounts = new Dictionary<ItemType, int>(); // Tracks items selected per type

        while (unchangedPhases < phaseOrder.Count && bestSolution.RemainingBudget >= minPrice)
        {
            var currentType = phaseOrder[phase % phaseOrder.Count];
            if (currentCounts.GetValueOrDefault(currentType, 0) >= maxPerType[currentType])
            {
                unchangedPhases++;
                phase++;
                continue;
            }

            // Define item count constraints for this phase
            var phaseConstraints = new Dictionary<ItemType, int>();
            for (int i = 0; i <= phase % phaseOrder.Count; i++)
            {
                var type = phaseOrder[i];
                int count = (i == phase % phaseOrder.Count) ? currentCounts.GetValueOrDefault(type, 0) + 1 : currentCounts.GetValueOrDefault(type, 0);
                phaseConstraints[type] = Math.Min(count, maxPerType[type]);
            }

            // Run knapsack with full budget and phase constraints
            var newSolution = RunKnapsack(dpItems, budget, phaseConstraints, priorityWeights);
            if (newSolution != null && newSolution.TotalProfit > bestSolution.TotalProfit)
            {
                bestSolution = newSolution;
                currentCounts = newSolution.CategoryCounts;
                unchangedPhases = 0;
                Console.WriteLine($"Phase {phase + 1}: Added item for {currentType}, Profit={bestSolution.TotalProfit}, Budget Used={budget - bestSolution.RemainingBudget}");
            }
            else
            {
                unchangedPhases++;
                maxPerType[currentType] = currentCounts.GetValueOrDefault(currentType, 0);
                Console.WriteLine($"Phase {phase + 1}: Failed to add for {currentType}, Fixed Max={maxPerType[currentType]}");
            }

            phase++;
        }

        var selectedItems = bestSolution.SelectedItems.Select(i => items.First(x => x.GlobalId == i.GlobalId && x.PlaceType == i.PlaceType)).ToList();
        Console.WriteLine($"Optimization Complete: Selected {selectedItems.Count} items, Total Profit={bestSolution.TotalProfit}, Total Cost={budget - bestSolution.RemainingBudget}");
        return selectedItems;
    }

    private KnapsackState RunKnapsack(
        List<DpItem> items,
        int budget,
        Dictionary<ItemType, int> phaseConstraints,
        Dictionary<ItemType, double> priorityWeights)
    {
        var states = new List<KnapsackState> { new KnapsackState { RemainingBudget = budget } };
        var bestState = states[0];
        var usedItemIds = new HashSet<string>();

        foreach (var item in items)
        {
            if (usedItemIds.Contains(item.Original.GlobalId)) continue;
            var itemType = item.PlaceType;
            if (!phaseConstraints.ContainsKey(itemType)) continue;
            if (states.Any(s => s.CategoryCounts.GetValueOrDefault(itemType, 0) >= phaseConstraints[itemType])) continue;
            if (item.Weight > budget) continue;

            var newStates = new List<KnapsackState>();
            foreach (var state in states)
            {
                if (state.RemainingBudget < item.Weight) continue;
                if (state.CategoryCounts.GetValueOrDefault(itemType, 0) >= phaseConstraints[itemType]) continue;

                var newState = state.Clone();
                double weightedProfit = item.Profit * priorityWeights.GetValueOrDefault(itemType, 1.0);
                newState.TotalProfit += weightedProfit;
                newState.RemainingBudget -= item.Weight;
                newState.SelectedItems.Add(item);
                newState.CategoryCounts[itemType] = newState.CategoryCounts.GetValueOrDefault(itemType, 0) + 1;
                newStates.Add(newState);
            }

            if (newStates.Any())
            {
                states.AddRange(newStates);
                var currentBest = states.OrderByDescending(s => s.TotalProfit).First();
                if (currentBest.TotalProfit > bestState.TotalProfit)
                    bestState = currentBest;
                usedItemIds.Add(item.Original.GlobalId);
            }
        }

        return bestState.SelectedItems.Any() ? bestState : null;
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