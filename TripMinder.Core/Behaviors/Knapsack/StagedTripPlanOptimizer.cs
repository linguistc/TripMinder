namespace TripMinder.Core.Behaviors.Knapsack;

public interface IStagedTripPlanOptimizer
{
    List<Item> OptimizeStaged(
        List<Item> items,
        Queue<string> interests,
        int initialBudget,
        UserDefinedKnapsackConstraints originalConstraints,
        (int a, int f, int e, int t)? priorities = null);
}

public class StagedTripPlanOptimizer : IStagedTripPlanOptimizer
{
    private readonly IKnapsackDP _dpCalculator;
    private readonly IKnapsackBacktracker _backtracker;

    public StagedTripPlanOptimizer(IKnapsackDP dpCalculator, IKnapsackBacktracker backtracker)
    {
        _dpCalculator = dpCalculator;
        _backtracker = backtracker;
    }

    public List<Item> OptimizeStaged(
        List<Item> items,
        Queue<string> interests,
        int initialBudget,
        UserDefinedKnapsackConstraints originalConstraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        var result = new List<Item>();
        int remainingBudget = initialBudget;
        var constraints = new UserDefinedKnapsackConstraints(
            originalConstraints.MaxRestaurants,
            originalConstraints.MaxAccommodations,
            originalConstraints.MaxEntertainments,
            originalConstraints.MaxTourismAreas);
        var currentCounts = new Dictionary<ItemType, int>
        {
            { ItemType.Restaurant, 0 },
            { ItemType.Accommodation, 0 },
            { ItemType.Entertainment, 0 },
            { ItemType.TourismArea, 0 }
        };

        Func<string, ItemType?> toType = s => s?.Trim().ToLowerInvariant() switch
        {
            "accommodation" => ItemType.Accommodation,
            "food" or "restaurants" => ItemType.Restaurant,
            "entertainment" or "entertainments" => ItemType.Entertainment,
            "tourism" or "tourismareas" => ItemType.TourismArea,
            _ => null
        };

        var orderedInterests = interests.Select((interest, index) => (interest, priority: interests.Count - index))
            .OrderByDescending(x => x.priority)
            .Select(x => x.interest)
            .ToList();

        int stage = 1;
        bool completedFullCycle = false;
        while (remainingBudget > 0 && currentCounts.Any(kv => kv.Value < GetMaxCount(kv.Key, constraints)))
        {
            bool anyAddedInCycle = false;
            foreach (var interest in orderedInterests)
            {
                var type = toType(interest);
                if (type == null || currentCounts[type.Value] >= GetMaxCount(type.Value, constraints)) continue;

                var availableItems = items.Except(result).Where(i => i.PlaceType == type.Value).ToList();
                if (!availableItems.Any())
                {
                    Console.WriteLine($"Stage {stage}: No items available for {type.Value}");
                    continue;
                }

                var stageConstraints = new UserDefinedKnapsackConstraints(
                    type == ItemType.Restaurant ? currentCounts[ItemType.Restaurant] + 1 : currentCounts[ItemType.Restaurant],
                    type == ItemType.Accommodation ? currentCounts[ItemType.Accommodation] + 1 : currentCounts[ItemType.Accommodation],
                    type == ItemType.Entertainment ? currentCounts[ItemType.Entertainment] + 1 : currentCounts[ItemType.Entertainment],
                    type == ItemType.TourismArea ? currentCounts[ItemType.TourismArea] + 1 : currentCounts[ItemType.TourismArea]
                );

                Console.WriteLine($"Stage {stage}: Calculating DP for {type.Value}, Budget={remainingBudget}, Items={availableItems.Count}");
                var (dp, decision, ids) = _dpCalculator.Calculate(
                    remainingBudget,
                    availableItems,
                    stageConstraints.MaxRestaurants,
                    stageConstraints.MaxAccommodations,
                    stageConstraints.MaxEntertainments,
                    stageConstraints.MaxTourismAreas);

                Console.WriteLine($"Stage {stage}: DP Result for {type.Value}, DP[{remainingBudget},{stageConstraints.MaxRestaurants},{stageConstraints.MaxAccommodations},0,0]={dp[remainingBudget, stageConstraints.MaxRestaurants, stageConstraints.MaxAccommodations, 0, 0]}");

                var state = new KnapsackState(
                    remainingBudget,
                    stageConstraints.MaxRestaurants,
                    stageConstraints.MaxAccommodations,
                    stageConstraints.MaxEntertainments,
                    stageConstraints.MaxTourismAreas,
                    availableItems.Count - 1,
                    availableItems,
                    decision,
                    new List<Item>(),
                    null,
                    priorities);

                var selected = _backtracker.BacktrackSingleSolution(state);
                var newItem = selected.FirstOrDefault(i => i.PlaceType == type.Value);
                if (newItem != null && remainingBudget >= (int)newItem.AveragePricePerAdult)
                {
                    result.Add(newItem);
                    remainingBudget -= (int)newItem.AveragePricePerAdult;
                    currentCounts[type.Value]++;
                    anyAddedInCycle = true;
                    Console.WriteLine($"Stage {stage}: Added {newItem.Name} (Type={newItem.PlaceType}, Price={newItem.AveragePricePerAdult}, Score={newItem.Score}), Remaining Budget={remainingBudget}");
                }
                else
                {
                    Console.WriteLine($"Stage {stage}: No item selected for {type.Value}. Selected={selected.Count}, BudgetCheck={(newItem == null ? "No item" : (remainingBudget >= (int)newItem.AveragePricePerAdult).ToString())}");
                }
            }

            if (!anyAddedInCycle)
            {
                if (completedFullCycle)
                {
                    Console.WriteLine($"Stage {stage}: Completed a full cycle with no items added. Stopping.");
                    break;
                }
                completedFullCycle = true;
            }
            else
            {
                completedFullCycle = false; // Reset if we added something
            }

            stage++;
        }

        return result;
    }

    private bool CanAddMoreItems(List<Item> items, List<Item> selected, int remainingBudget, IKnapsackConstraints constraints, Dictionary<ItemType, int> currentCounts, List<string> orderedInterests, Func<string, ItemType?> toType)
    {
        var availableItems = items.Except(selected).ToList();
        var allowedTypes = orderedInterests.Select(i => toType(i)).Where(t => t.HasValue).Select(t => t.Value).ToHashSet();
        foreach (var item in availableItems)
        {
            if (!allowedTypes.Contains(item.PlaceType)) continue;
            if (currentCounts[item.PlaceType] >= GetMaxCount(item.PlaceType, constraints)) continue;
            if (remainingBudget >= (int)item.AveragePricePerAdult)
            {
                Console.WriteLine($"Can add item: {item.GlobalId} (Type={item.PlaceType}, Price={item.AveragePricePerAdult}, Score={item.Score})");
                return true;
            }
        }
        Console.WriteLine("No items can be added within remaining budget, constraints, or priorities.");
        return false;
    }

    private int GetMaxCount(ItemType itemType, IKnapsackConstraints constraints)
    {
        return itemType switch
        {
            ItemType.Restaurant => constraints.MaxRestaurants,
            ItemType.Accommodation => constraints.MaxAccommodations,
            ItemType.Entertainment => constraints.MaxEntertainments,
            ItemType.TourismArea => constraints.MaxTourismAreas,
            _ => 0
        };
    }
}