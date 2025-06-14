using TripMinder.Core.Behaviors.Shared;
namespace TripMinder.Core.Behaviors.Knapsack;

public class Solution
{
    public List<Item> Items { get; set; } = new List<Item>();
    public double TotalScore { get; set; }
    public double TotalCost { get; set; }
}

public record SolutionResponse(
    List<ItemResponse> Items,
    double TotalScore,
    double TotalCost
);

public class SolutionOptimizer
{
    private readonly List<(List<Item> Solution, double Score)> _topSolutions;
    private readonly List<HashSet<int>> _usedItemsPerSolution;
    private readonly int _maxSolutions;

    public SolutionOptimizer(int maxSolutions = 10)
    {
        _topSolutions = new List<(List<Item>, double)>();
        _usedItemsPerSolution = new List<HashSet<int>>();
        _maxSolutions = maxSolutions;
    }

    public void TryAddSolution(List<Item> solution)
    {
        if (!solution.Any()) return;

        double score = solution.Sum(item => item.Score);

        // Diversity Constraint: تحقق إن الـ Solution مش متشابهة زيادة
        bool isUnique = true;
        foreach (var existingItems in _usedItemsPerSolution)
        {
            var intersection = existingItems.Intersect(solution.Select(i => i.Id)).Count();
            if (intersection > solution.Count / 2)
            {
                isUnique = false;
                break;
            }
        }

        if (isUnique)
        {
            _topSolutions.Add((solution, score));
            _usedItemsPerSolution.Add(new HashSet<int>(solution.Select(i => i.Id)));

            // إزالة أضعف Solution لو تجاوزنا الحد
            if (_topSolutions.Count > _maxSolutions)
            {
                var minIndex = _topSolutions.IndexOf(_topSolutions.OrderBy(s => s.Score).First());
                _topSolutions.RemoveAt(minIndex);
                _usedItemsPerSolution.RemoveAt(minIndex);
            }
        }
    }

    public List<List<Item>> GetTopSolutions()
    {
        return _topSolutions
            .OrderByDescending(s => s.Score)
            .Select(s => s.Solution)
            .ToList();
    }
}