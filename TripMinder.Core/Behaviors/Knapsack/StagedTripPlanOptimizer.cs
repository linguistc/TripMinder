using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class StagedTripPlanOptimizer : IStagedTripPlanOptimizer
{
    private readonly IDynamicProgrammingCalculator _dpCalculator;
    private readonly IProfitFinder _profitFinder;
    private readonly IKnapsackBacktracker _backtracker;

    public StagedTripPlanOptimizer(
        IDynamicProgrammingCalculator dpCalculator,
        IProfitFinder profitFinder,
        IKnapsackBacktracker backtracker)
    {
        _dpCalculator = dpCalculator;
        _profitFinder = profitFinder;
        _backtracker = backtracker;
    }

    public async Task<List<Item>> OptimizeStagedAsync(
        List<Item> items,
        List<string> orderedInterests,
        int budget,
        UserDefinedKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        // 1. Check cheapest item
        double minPrice = items.Any() ? items.Min(i => i.AveragePricePerAdult) : double.MaxValue;
        if (budget < minPrice)
            return new List<Item>();

        // 2. Prepare DP items and phase order
        var phaseOrder = orderedInterests
            .Select(s => GetItemType(s))
            .Distinct()
            .ToList();

        var dpItems = items.Select(i => new DpItem(i)).ToList();

        // 3. Initialize best solution trackers
        var bestProfit = 0.0f;
        var bestCounts = phaseOrder.ToDictionary(t => t, t => 0);
        int finalUsedBudget = 0;
        int phase = 0;
        bool initialCoverageComplete = false;

        // 4. Phased expansion loop
        while (true)
        {
            bool madeProgress = false;

            // Phase 1: Cover each interest type once in order
            if (phase < phaseOrder.Count)
            {
                var type = phaseOrder[phase];
                int maxForType = GetMaxConstraint(constraints, type);
                if (bestCounts[type] >= maxForType)
                {
                    phase++;
                    continue;
                }

                // Force adding one item of the current type
                var phaseLimits = phaseOrder.ToDictionary(
                    t => t,
                    t => t == type ? Math.Min(bestCounts[t] + 1, maxForType) : bestCounts[t]
                );

                var (dp, decision, _) = _dpCalculator.Calculate(
                    budget,
                    dpItems,
                    phaseLimits.GetValueOrDefault(ItemType.Restaurant),
                    phaseLimits.GetValueOrDefault(ItemType.Accommodation),
                    phaseLimits.GetValueOrDefault(ItemType.Entertainment),
                    phaseLimits.GetValueOrDefault(ItemType.TourismArea)
                );

                var (phaseProfit, usedBudget, r, a, e, t) = _profitFinder.FindMaxProfit(
                    dp,
                    budget,
                    new UserDefinedKnapsackConstraints(
                        phaseLimits.GetValueOrDefault(ItemType.Restaurant),
                        phaseLimits.GetValueOrDefault(ItemType.Accommodation),
                        phaseLimits.GetValueOrDefault(ItemType.Entertainment),
                        phaseLimits.GetValueOrDefault(ItemType.TourismArea)
                    ),
                    requireExact: false
                );

                if (phaseProfit > bestProfit)
                {
                    bestProfit = phaseProfit;
                    bestCounts[ItemType.Restaurant] = r;
                    bestCounts[ItemType.Accommodation] = a;
                    bestCounts[ItemType.Entertainment] = e;
                    bestCounts[ItemType.TourismArea] = t;
                    finalUsedBudget = usedBudget;
                    madeProgress = true;
                    Console.WriteLine($"Phase {phase}: Added {type}, Profit={bestProfit}, Counts={string.Join(",", bestCounts)}, UsedBudget={finalUsedBudget}");
                }

                phase++;
                if (phase == phaseOrder.Count)
                    initialCoverageComplete = true;
            }
            // Phase 2: Expand freely within constraints
            else if (initialCoverageComplete)
            {
                int unchangedRounds = 0;
                while (unchangedRounds < 1)
                {
                    bool phaseProgress = false;

                    foreach (var type in phaseOrder)
                    {
                        int maxForType = GetMaxConstraint(constraints, type);
                        if (bestCounts[type] >= maxForType)
                            continue;

                        var phaseLimits = phaseOrder.ToDictionary(
                            t => t,
                            t => Math.Min(
                                t == type ? bestCounts[t] + 1 : bestCounts[t],
                                maxForType)
                        );

                        var (dp, decision, _) = _dpCalculator.Calculate(
                            budget,
                            dpItems,
                            phaseLimits.GetValueOrDefault(ItemType.Restaurant),
                            phaseLimits.GetValueOrDefault(ItemType.Accommodation),
                            phaseLimits.GetValueOrDefault(ItemType.Entertainment),
                            phaseLimits.GetValueOrDefault(ItemType.TourismArea)
                        );

                        var (phaseProfit, usedBudget, r, a, e, t) = _profitFinder.FindMaxProfit(
                            dp,
                            budget,
                            new UserDefinedKnapsackConstraints(
                                phaseLimits.GetValueOrDefault(ItemType.Restaurant),
                                phaseLimits.GetValueOrDefault(ItemType.Accommodation),
                                phaseLimits.GetValueOrDefault(ItemType.Entertainment),
                                phaseLimits.GetValueOrDefault(ItemType.TourismArea)
                            ),
                            requireExact: false
                        );

                        if (phaseProfit > bestProfit)
                        {
                            bestProfit = phaseProfit;
                            bestCounts[ItemType.Restaurant] = r;
                            bestCounts[ItemType.Accommodation] = a;
                            bestCounts[ItemType.Entertainment] = e;
                            bestCounts[ItemType.TourismArea] = t;
                            finalUsedBudget = usedBudget;
                            phaseProgress = true;
                            Console.WriteLine($"Free Phase: Added {type}, Profit={bestProfit}, Counts={string.Join(",", bestCounts)}, UsedBudget={finalUsedBudget}");
                        }
                    }

                    if (!phaseProgress)
                        unchangedRounds++;
                    else
                        unchangedRounds = 0;
                }
                break; // Exit after free expansion
            }
        }

        // 5. Final backtracking using used budget
        var finalLimits = phaseOrder.ToDictionary(t => t, t => bestCounts.GetValueOrDefault(t));
        var (finalDp, finalDecision, _) = _dpCalculator.Calculate(
            budget,
            dpItems,
            finalLimits.GetValueOrDefault(ItemType.Restaurant),
            finalLimits.GetValueOrDefault(ItemType.Accommodation),
            finalLimits.GetValueOrDefault(ItemType.Entertainment),
            finalLimits.GetValueOrDefault(ItemType.TourismArea)
        );

        var (finalProfit, fusedBudget, fr, fa, fe, ft) = _profitFinder.FindMaxProfit(
            finalDp,
            budget,
            new UserDefinedKnapsackConstraints(
                finalLimits.GetValueOrDefault(ItemType.Restaurant),
                finalLimits.GetValueOrDefault(ItemType.Accommodation),
                finalLimits.GetValueOrDefault(ItemType.Entertainment),
                finalLimits.GetValueOrDefault(ItemType.TourismArea)
            ),
            requireExact: true
        );

        Console.WriteLine($"Backtracking from: Budget={fusedBudget}, R={fr}, A={fa}, E={fe}, T={ft}");

        if (finalDp[fusedBudget, fr, fa, fe, ft] == float.MinValue)
        {
            Console.WriteLine("❌ Attempting to backtrack from invalid DP state!");
            return new List<Item>();
        }

        var finalState = new KnapsackState
        {
            TotalProfit = finalProfit,
            RemainingBudget = finalUsedBudget,
            CategoryCounts = new Dictionary<ItemType, int>
            {
                [ItemType.Restaurant] = fr,
                [ItemType.Accommodation] = fa,
                [ItemType.Entertainment] = fe,
                [ItemType.TourismArea] = ft
            },
            Priorities = priorities
        };

        var selectedDp = _backtracker.BacktrackSingleSolution(finalState, dpItems, finalDecision);
        return selectedDp.Select(d => d.Original).ToList();
    }

    private int GetMaxConstraint(UserDefinedKnapsackConstraints c, ItemType t) => t switch
    {
        ItemType.Restaurant => c.MaxRestaurants,
        ItemType.Accommodation => c.MaxAccommodations,
        ItemType.Entertainment => c.MaxEntertainments,
        ItemType.TourismArea => c.MaxTourismAreas,
        _ => 0
    };

    private ItemType GetItemType(string interest) => interest?.Trim().ToLowerInvariant() switch
    {
        "accommodation" => ItemType.Accommodation,
        "restaurants" or "food" => ItemType.Restaurant,
        "entertainments" or "entertainment" => ItemType.Entertainment,
        "tourismareas" or "tourism" => ItemType.TourismArea,
        _ => throw new ArgumentException($"Unknown interest: {interest}")
    };
    
    
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

}