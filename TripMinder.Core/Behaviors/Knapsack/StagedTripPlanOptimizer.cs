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
        while (remainingBudget > 0 && currentCounts.Any(kv => kv.Value < GetMaxCount(kv.Key, constraints)))
        {
            bool added = false;
            foreach (var interest in orderedInterests.ToList())
            {
                var type = toType(interest);
                if (type == null || currentCounts[type.Value] >= GetMaxCount(type.Value, constraints)) continue;

                var availableItems = items.Except(result).ToList();
                if (!availableItems.Any(i => i.PlaceType == type.Value)) continue;

                float priorityWeight = 1.2f - (stage - 1) * 0.1f;
                foreach (var item in availableItems.Where(i => i.PlaceType == type.Value))
                {
                    item.Score *= priorityWeight;
                }

                int exactR = type == ItemType.Restaurant ? currentCounts[ItemType.Restaurant] + 1 : currentCounts[ItemType.Restaurant];
                int exactA = type == ItemType.Accommodation ? currentCounts[ItemType.Accommodation] + 1 : currentCounts[ItemType.Accommodation];
                int exactE = type == ItemType.Entertainment ? currentCounts[ItemType.Entertainment] + 1 : currentCounts[ItemType.Entertainment];
                int exactT = type == ItemType.TourismArea ? currentCounts[ItemType.TourismArea] + 1 : currentCounts[ItemType.TourismArea];

                var (dp, decision, ids) = _dpCalculator.Calculate(
                    remainingBudget,
                    availableItems,
                    exactR,
                    exactA,
                    exactE,
                    exactT);

                var state = new KnapsackState(
                    remainingBudget,
                    exactR,
                    exactA,
                    exactE,
                    exactT,
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
                    items.RemoveAll(i => i.GlobalId == newItem.GlobalId);
                    added = true;
                    Console.WriteLine($"Stage {stage}: Added {newItem.Name} (Type={newItem.PlaceType}, Price={newItem.AveragePricePerAdult}, Score={newItem.Score}), Remaining Budget={remainingBudget}");
                }

                foreach (var item in availableItems.Where(i => i.PlaceType == type.Value))
                {
                    item.Score /= priorityWeight;
                }
            }

            // إعادة تقييم العناصر السابقة
            if (added && result.Count > 1)
            {
                var currentProfit = result.Sum(i => i.Score);
                var tempResult = new List<Item>();
                var tempBudget = initialBudget;
                var tempCounts = new Dictionary<ItemType, int>
                {
                    { ItemType.Restaurant, 0 },
                    { ItemType.Accommodation, 0 },
                    { ItemType.Entertainment, 0 },
                    { ItemType.TourismArea, 0 }
                };
                var tempItems = items.ToList();

                foreach (var interest in orderedInterests.ToList())
                {
                    var type = toType(interest);
                    if (type == null || tempCounts[type.Value] >= GetMaxCount(type.Value, constraints)) continue;

                    var availableItems = tempItems.Except(tempResult).ToList();
                    if (!availableItems.Any(i => i.PlaceType == type.Value)) continue;

                    float priorityWeight = 1.2f - (tempCounts.Sum(c => c.Value) - 1) * 0.1f;
                    foreach (var item in availableItems.Where(i => i.PlaceType == type.Value))
                    {
                        item.Score *= priorityWeight;
                    }

                    int exactR = type == ItemType.Restaurant ? tempCounts[ItemType.Restaurant] + 1 : tempCounts[ItemType.Restaurant];
                    int exactA = type == ItemType.Accommodation ? tempCounts[ItemType.Accommodation] + 1 : tempCounts[ItemType.Accommodation];
                    int exactE = type == ItemType.Entertainment ? tempCounts[ItemType.Entertainment] + 1 : tempCounts[ItemType.Entertainment];
                    int exactT = type == ItemType.TourismArea ? tempCounts[ItemType.TourismArea] + 1 : tempCounts[ItemType.TourismArea];

                    var (dp, decision, ids) = _dpCalculator.Calculate(
                        tempBudget,
                        availableItems,
                        exactR,
                        exactA,
                        exactE,
                        exactT);

                    var state = new KnapsackState(
                        tempBudget,
                        exactR,
                        exactA,
                        exactE,
                        exactT,
                        availableItems.Count - 1,
                        availableItems,
                        decision,
                        new List<Item>(),
                        null,
                        priorities);

                    var selected = _backtracker.BacktrackSingleSolution(state);
                    var newItem = selected.FirstOrDefault(i => i.PlaceType == type.Value);
                    if (newItem != null && tempBudget >= (int)newItem.AveragePricePerAdult)
                    {
                        tempResult.Add(newItem);
                        tempBudget -= (int)newItem.AveragePricePerAdult;
                        tempCounts[type.Value]++;
                        tempItems.RemoveAll(i => i.GlobalId == newItem.GlobalId);
                    }

                    foreach (var item in availableItems.Where(i => i.PlaceType == type.Value))
                    {
                        item.Score /= priorityWeight;
                    }
                }

                var newProfit = tempResult.Sum(i => i.Score);
                if (newProfit > currentProfit && tempResult.Count >= result.Count)
                {
                    result = tempResult;
                    remainingBudget = tempBudget;
                    currentCounts = tempCounts;
                    items = tempItems;
                    Console.WriteLine($"Re-evaluated: New Profit={newProfit}, Old Profit={currentProfit}, New Items={string.Join(", ", result.Select(i => i.Name))}");
                }
            }

            if (!added) break;
            stage++;
        }

        return result;
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