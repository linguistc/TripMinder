using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class GreedySolutionCollector
{
    private readonly List<(List<Item> Plan, double Score)> _topPlans;
    private readonly List<HashSet<int>> _usedItemsPerPlan;
    private readonly int _maxPlans;

    public GreedySolutionCollector(int maxPlans = 10)
    {
        _topPlans = new List<(List<Item>, double)>();
        _usedItemsPerPlan = new List<HashSet<int>>();
        _maxPlans = maxPlans;
    }

    public void TryAddPlan(List<Item> plan)
    {
        if (!plan.Any()) return;

        double score = plan.Sum(item => item.Score);
        double cost = plan.Sum(item => item.AveragePricePerAdult);

        bool isUnique = true;
        foreach (var existingItems in _usedItemsPerPlan)
        {
            var intersection = existingItems.Intersect(plan.Select(i => i.Id)).Count();
            if (intersection > plan.Count / 2)
            {
                isUnique = false;
                break;
            }
        }

        if (isUnique)
        {
            _topPlans.Add((plan, score));
            _usedItemsPerPlan.Add(new HashSet<int>(plan.Select(i => i.Id)));

            if (_topPlans.Count > _maxPlans)
            {
                var minIndex = _topPlans.IndexOf(_topPlans.OrderBy(s => s.Score).First());
                _topPlans.RemoveAt(minIndex);
                _usedItemsPerPlan.RemoveAt(minIndex);
            }

            Console.WriteLine($"Added Plan: Score={score}, Cost={cost}, Items={string.Join(", ", plan.Select(i => i.Name))}");
        }
    }

    public List<List<Item>> GetTopPlans()
    {
        return _topPlans
            .OrderByDescending(s => s.Score)
            .Select(s => s.Plan)
            .ToList();
    }
}