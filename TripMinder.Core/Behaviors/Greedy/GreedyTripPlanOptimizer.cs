using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class GreedyTripPlanOptimizer : IGreedyStagedTripPlanOptimizer
{
    private readonly IItemFetcher _itemFetcher;

    public GreedyTripPlanOptimizer(IItemFetcher itemFetcher)
    {
        _itemFetcher = itemFetcher;
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

        // 2. Prepare phase order and item groups
        var phaseOrder = orderedInterests
            .Select(GetItemType)
            .Distinct()
            .ToList();
        Console.WriteLine($"Phase Order: {string.Join(", ", phaseOrder)}");

        // Group and sort items by weighted profit-to-cost ratio
        var itemGroups = items
            .GroupBy(i => i.PlaceType)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(i => priorities.HasValue ? CalculateWeightedScore(i, priorities.Value) : i.Score / Math.Max(i.AveragePricePerAdult, 0.01))
                    .ToList()
            );
        Console.WriteLine($"Item Groups: {string.Join(", ", itemGroups.Keys.Select(k => $"{k} ({itemGroups[k].Count} items)"))}");

        // 3. Initialize state
        var selectedItems = new List<Item>();
        var countTracker = new CountTracker();
        var remainingBudget = budget;
        var pointers = phaseOrder.ToDictionary(t => t, _ => 0);
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
                if (countTracker.Exceeded(type, constraints))
                {
                    Console.WriteLine($"Phase {phase}: Skipping {type} (max reached: {GetMaxConstraint(constraints, type)})");
                    phase++;
                    continue;
                }

                if (TryAddItem(type, itemGroups, pointers, countTracker, constraints, ref remainingBudget, selectedItems))
                {
                    madeProgress = true;
                    Console.WriteLine($"Phase {phase}: Added {type}, Count={countTracker.GetCount(type)}, Budget={remainingBudget}, Item={selectedItems.Last().Name}");
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
                        if (countTracker.Exceeded(type, constraints))
                            continue;

                        if (TryAddItem(type, itemGroups, pointers, countTracker, constraints, ref remainingBudget, selectedItems))
                        {
                            phaseProgress = true;
                            Console.WriteLine($"Free Phase: Added {type}, Count={countTracker.GetCount(type)}, Budget={remainingBudget}, Item={selectedItems.Last().Name}");
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
        var totalProfit = selectedItems.Sum(i => i.Score);
        var totalCost = selectedItems.Sum(i => i.AveragePricePerAdult);
        Console.WriteLine($"Greedy Optimization Complete: Selected {selectedItems.Count} items, Total Profit={totalProfit}, Total Cost={totalCost}, Remaining Budget={remainingBudget}");
        return selectedItems;
    }

    private bool TryAddItem(
        ItemType type,
        Dictionary<ItemType, List<Item>> itemGroups,
        Dictionary<ItemType, int> pointers,
        CountTracker countTracker,
        IKnapsackConstraints constraints,
        ref int remainingBudget,
        List<Item> selectedItems)
    {
        if (!itemGroups.ContainsKey(type) || pointers[type] >= itemGroups[type].Count)
        {
            Console.WriteLine($"No more {type} items available.");
            return false;
        }

        var item = itemGroups[type][pointers[type]];
        if (item.AveragePricePerAdult <= remainingBudget)
        {
            selectedItems.Add(item);
            remainingBudget -= (int)item.AveragePricePerAdult;
            countTracker.Increment(type);
            pointers[type]++;
            return true;
        }

        Console.WriteLine($"Item {item.Name} (Type={type}, Cost={item.AveragePricePerAdult}) exceeds remaining budget {remainingBudget}.");
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

    private ItemType GetItemType(string interest) => interest?.Trim().ToLowerInvariant() switch
    {
        "accommodation" => ItemType.Accommodation,
        "restaurants" or "food" => ItemType.Restaurant,
        "entertainments" or "entertainment" => ItemType.Entertainment,
        "tourismareas" or "tourism" => ItemType.TourismArea,
        _ => throw new ArgumentException($"Unknown interest: {interest}")
    };

    private int GetMaxConstraint(IKnapsackConstraints c, ItemType t) => t switch
    {
        ItemType.Restaurant => c.MaxRestaurants,
        ItemType.Accommodation => c.MaxAccommodations,
        ItemType.Entertainment => c.MaxEntertainments,
        ItemType.TourismArea => c.MaxTourismAreas,
        _ => 0
    };
}