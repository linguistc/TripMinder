using System;
using System.Collections.Generic;
using System.Linq;
using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackSolver : IKnapsackSolver
{
    private readonly IKnapsackDP _dpCalculator;
    private readonly IKnapsackBacktracker _backtracker;
    private readonly IProfitFinder _profitFinder;

    public KnapsackSolver(IKnapsackDP dpCalculator, IKnapsackBacktracker backtracker, IProfitFinder profitFinder)
    {
        _dpCalculator = dpCalculator;
        _backtracker = backtracker;
        _profitFinder = profitFinder;
    }

    // Note: This solver is used for non-phased optimization (e.g., OptimizePlanMultiple).
    // For phased incremental knapsack, StagedTripPlanOptimizer uses a state-based approach.
    public (float maxProfit, List<Item> selectedItems) GetMaxProfit(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null,
        bool requireExact = false)
    {
        var dpItems = items.Select(i => new DpItem(i)).ToList();
        var (dp, decision, itemIds) = _dpCalculator.Calculate(
            budget, dpItems,
            constraints.MaxRestaurants,
            constraints.MaxAccommodations,
            constraints.MaxEntertainments,
            constraints.MaxTourismAreas);

        var (maxProfit, usedBudget, finalR, finalA, finalE, finalT) =
            _profitFinder.FindMaxProfit(dp, budget, constraints, requireExact);
        Console.WriteLine(
            $"Max Profit: {maxProfit}, Final State: Restaurants={finalR}, Accommodations={finalA}, Entertainments={finalE}, TourismAreas={finalT}, Used Budget={usedBudget}");

        var state = new KnapsackState
        {
            TotalProfit = maxProfit,
            RemainingBudget = budget - usedBudget,
            CategoryCounts = new Dictionary<ItemType, int>
            {
                [ItemType.Restaurant] = finalR,
                [ItemType.Accommodation] = finalA,
                [ItemType.Entertainment] = finalE,
                [ItemType.TourismArea] = finalT
            },
            UsedCategories = new HashSet<ItemType>(),
            SelectedItems = new List<DpItem>(),
            Phase = 0,
            Priorities = priorities
        };

        var selectedDpItems = _backtracker.BacktrackSingleSolution(state, dpItems, decision);
        var selectedItems = selectedDpItems.Select(d => d.Original).ToList();

        Console.WriteLine(
            $"Selected Items Count: {selectedItems.Count}, Total Cost: {selectedItems.Sum(i => i.AveragePricePerAdult)}");
        return (maxProfit, selectedItems);
    }

    public (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null,
        bool requireExact = false)
    {
        var dpItems = items.Select(i => new DpItem(i)).ToList();
        var (dp, decision, itemIds) = _dpCalculator.Calculate(
            budget, dpItems,
            constraints.MaxRestaurants,
            constraints.MaxAccommodations,
            constraints.MaxEntertainments,
            constraints.MaxTourismAreas);

        var (maxProfit, usedBudget, finalR, finalA, finalE, finalT) =
            _profitFinder.FindMaxProfit(dp, budget, constraints, requireExact);
        Console.WriteLine(
            $"Max Profit: {maxProfit}, Final State: Restaurants={finalR}, Accommodations={finalA}, Entertainments={finalE}, TourismAreas={finalT}, Used Budget={usedBudget}");

        var state = new KnapsackState
        {
            TotalProfit = maxProfit,
            RemainingBudget = budget - usedBudget,
            CategoryCounts = new Dictionary<ItemType, int>
            {
                [ItemType.Restaurant] = finalR,
                [ItemType.Accommodation] = finalA,
                [ItemType.Entertainment] = finalE,
                [ItemType.TourismArea] = finalT
            },
            UsedCategories = new HashSet<ItemType>(),
            SelectedItems = new List<DpItem>(),
            Phase = 0,
            Priorities = priorities
        };

        var allSolutions = _backtracker.BacktrackTopSolutions(state, dpItems, decision, 10);
        var allSelectedItems = allSolutions.Select(solution => solution.Select(d => d.Original).ToList()).ToList();
        Console.WriteLine($"Total Solutions Found: {allSelectedItems.Count}");
        return (maxProfit, allSelectedItems);
    }
}