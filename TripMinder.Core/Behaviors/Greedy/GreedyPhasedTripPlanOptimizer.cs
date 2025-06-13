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
public class GreedyPhasedTripPlanOptimizer : IGreedyStagedTripPlanOptimizer
{
    public GreedyPhasedTripPlanOptimizer()
    {
        // No dependencies needed
    }

    public async Task<List<Item>> OptimizeStagedAsync(
        List<Item> items,
        List<string> orderedInterests,
        int budget,
        UserDefinedKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        // 1. Validate budget
        double minPrice = items.Any() ? items.Min(i => i.AveragePricePerAdult) : double.MaxValue;
        if (budget < minPrice)
        {
            Console.WriteLine($"Budget {budget} is too low. Minimum price: {minPrice}");
            return new List<Item>();
        }

        // 2. Prepare phase order and greedy lists
        var phaseOrder = orderedInterests
            .Select(MapToItemType)
            .Distinct()
            .ToList();
        Console.WriteLine($"Phase Order: {string.Join(", ", phaseOrder)}");

        // Group and sort items by weighted score-to-cost ratio
        var greedyLists = phaseOrder.ToDictionary(
            t => t,
            t => new Queue<Item>(
                items
                    .Where(i => i.PlaceType == t)
                    .OrderByDescending(i => priorities.HasValue ? CalculateWeightedScore(i, priorities.Value) : i.Score / Math.Max(i.AveragePricePerAdult, 0.01))
            )
        );
        Console.WriteLine($"Greedy Lists: {string.Join(", ", greedyLists.Keys.Select(k => $"{k} ({greedyLists[k].Count} items)"))}");

        // 3. Initialize state
        var selected = new List<Item>();
        var tracker = new CountTracker();
        int remainingBudget = budget;
        bool initialCoverageComplete = false;
        int phase = 0;

        // 4. Phased expansion loop
        while (true)
        {
            bool madeProgress = false;

            // Phase 1: Cover each interest type once
            if (phase < phaseOrder.Count)
            {
                var type = phaseOrder[phase];
                if (tracker.Exceeded(type, constraints))
                {
                    Console.WriteLine($"Phase {phase}: Skipping {type} (max reached: {GetMax(constraints, type)})");
                    phase++;
                    continue;
                }

                if (TryTakeNext(type, greedyLists, tracker, ref remainingBudget, constraints, selected))
                {
                    madeProgress = true;
                    Console.WriteLine($"Phase {phase}: Added {type}, Count={tracker.GetCount(type)}, Budget={remainingBudget}, Item={selected.Last().Name}");
                }
                else
                {
                    Console.WriteLine($"Phase {phase}: No suitable {type} item found for budget {remainingBudget}");
                }

                phase++;
                if (phase == phaseOrder.Count)
                {
                    initialCoverageComplete = true;
                    Console.WriteLine("Initial coverage complete. Entering free expansion phase.");
                }
            }
            // Phase 2: Expand freely within constraints
            else if (initialCoverageComplete)
            {
                int unchangedRounds = 0;
                while (unchangedRounds < phaseOrder.Count)
                {
                    bool phaseProgress = false;

                    foreach (var type in phaseOrder)
                    {
                        if (tracker.Exceeded(type, constraints))
                            continue;

                        if (TryTakeNext(type, greedyLists, tracker, ref remainingBudget, constraints, selected))
                        {
                            phaseProgress = true;
                            Console.WriteLine($"Free Phase: Added {type}, Count={tracker.GetCount(type)}, Budget={remainingBudget}, Item={selected.Last().Name}");
                        }
                    }

                    if (!phaseProgress)
                        unchangedRounds++;
                    else
                        unchangedRounds = 0;
                }
                Console.WriteLine("Free expansion complete. No further items can be added.");
                break;
            }
        }

        // 5. Log final result
        var totalScore = selected.Sum(i => i.Score);
        var totalCost = selected.Sum(i => i.AveragePricePerAdult);
        Console.WriteLine($"Greedy Phased Optimization Complete: Selected {selected.Count} items, Total Score={totalScore}, Total Cost={totalCost}, Remaining Budget={remainingBudget}");
        return selected;
    }

    private bool TryTakeNext(
        ItemType type,
        Dictionary<ItemType, Queue<Item>> lists,
        CountTracker tracker,
        ref int remainingBudget,
        UserDefinedKnapsackConstraints constraints,
        List<Item> selected)
    {
        if (!lists.ContainsKey(type) || !lists[type].Any())
        {
            Console.WriteLine($"No more {type} items available.");
            return false;
        }

        var candidate = lists[type].Peek();
        if (candidate.AveragePricePerAdult <= remainingBudget)
        {
            lists[type].Dequeue();
            selected.Add(candidate);
            remainingBudget -= (int)candidate.AveragePricePerAdult;
            tracker.Increment(type);
            return true;
        }

        Console.WriteLine($"Item {candidate.Name} (Type={type}, Cost={candidate.AveragePricePerAdult}) exceeds remaining budget {remainingBudget}.");
        return false;
    }

    private double CalculateWeightedScore(Item item, (int a, int f, int e, int t) priorities)
    {
        double priorityWeight = item.PlaceType switch
        {
            ItemType.Accommodation => 1.0 + priorities.a * 0.5,
            ItemType.Restaurant => 1.0 + priorities.f * 0.5,
            ItemType.Entertainment => 1.0 + priorities.e * 0.5,
            ItemType.TourismArea => 1.0 + priorities.t * 0.5,
            _ => 1.0
        };
        return (item.Score * priorityWeight) / Math.Max(item.AveragePricePerAdult, 0.01);
    }

    private int GetMax(UserDefinedKnapsackConstraints c, ItemType t) => t switch
    {
        ItemType.Accommodation => c.MaxAccommodations,
        ItemType.Restaurant => c.MaxRestaurants,
        ItemType.Entertainment => c.MaxEntertainments,
        ItemType.TourismArea => c.MaxTourismAreas,
        _ => 0
    };

    private ItemType MapToItemType(string s) => s?.Trim().ToLowerInvariant() switch
    {
        "accommodation" => ItemType.Accommodation,
        "restaurants" or "food" => ItemType.Restaurant,
        "entertainments" or "entertainment" => ItemType.Entertainment,
        "tourismareas" or "tourism" => ItemType.TourismArea,
        _ => throw new ArgumentException($"Unknown interest: {s}")
    };
}