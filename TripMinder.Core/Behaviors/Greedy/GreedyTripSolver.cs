using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class GreedyTripSolver : IGreedyTripSolver
{
    private readonly GreedySolutionCollector _collector;

    public GreedyTripSolver(GreedySolutionCollector collector)
    {
        _collector = collector;
    }

    public (float maxProfit, List<Item> selectedItems) GetBestPlan(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        var selectedItems = RunGreedy(budget, items, constraints, priorities);
        float maxProfit = selectedItems.Sum(i => i.Score);
        Console.WriteLine($"Greedy Single Plan: Profit={maxProfit}, Items={selectedItems.Count}, Cost={selectedItems.Sum(i => i.AveragePricePerAdult)}");
        return (maxProfit, selectedItems);
    }

    public (float maxProfit, List<List<Item>> allPlans) GetMultiplePlans(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null)
    {
        var basePlan = RunGreedy(budget, items, constraints, priorities);
        _collector.TryAddPlan(basePlan);

        if (priorities.HasValue)
        {
            for (int i = 0; i < 5; i++)
            {
                var rand = new Random(i);
                var perturbedPriorities = (
                    a: priorities.Value.a + (float)(rand.NextDouble() * 0.2 - 0.1),
                    f: priorities.Value.f + (float)(rand.NextDouble() * 0.2 - 0.1),
                    e: priorities.Value.e + (float)(rand.NextDouble() * 0.2 - 0.1),
                    t: priorities.Value.t + (float)(rand.NextDouble() * 0.2 - 0.1)
                );
                var variantPlan = RunGreedy(budget, items, constraints, perturbedPriorities);
                _collector.TryAddPlan(variantPlan);
            }
        }

        var allPlans = _collector.GetTopPlans();
        float maxProfit = allPlans.Any() ? allPlans.Max(s => s.Sum(i => i.Score)) : 0f;
        Console.WriteLine($"Greedy Multiple Plans: Count={allPlans.Count}, Max Profit={maxProfit}");
        return (maxProfit, allPlans);
    }

    private List<Item> RunGreedy(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (float a, float f, float e, float t)? priorities)
    {
        var tracker = new CountTracker();
        var priorityWeights = new Dictionary<ItemType, float>
        {
            [ItemType.Accommodation] = priorities?.a ?? 0.1f,
            [ItemType.Restaurant] = priorities?.f ?? 0.1f,
            [ItemType.Entertainment] = priorities?.e ?? 0.1f,
            [ItemType.TourismArea] = priorities?.t ?? 0.1f
        };

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

        var selectedItems = new List<Item>();
        int remainingBudget = budget;

        while (remainingBudget > 0)
        {
            var candidate = itemScores
                .FirstOrDefault(x =>
                    !selectedItems.Contains(x.Item) &&
                    x.Item.AveragePricePerAdult <= remainingBudget &&
                    !tracker.Exceeded(x.Item.PlaceType, constraints));

            if (candidate == null) break;

            selectedItems.Add(candidate.Item);
            remainingBudget -= (int)candidate.Item.AveragePricePerAdult;
            tracker.Increment(candidate.Item.PlaceType);
            Console.WriteLine($"Selected: {candidate.Item.Name}, Type={candidate.Item.PlaceType}, Score={candidate.Item.Score}, Cost={candidate.Item.AveragePricePerAdult}");
        }

        return selectedItems;
    }
}