using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class GreedySolutionOptimizer
{
    private readonly List<(List<Item> Solution, double Score)> _topSolutions;
    private readonly List<HashSet<int>> _usedItemsPerSolution;
    private readonly int _maxSolutions;

    public GreedySolutionOptimizer(int maxSolutions = 10)
    {
        _topSolutions = new List<(List<Item>, double)>();
        _usedItemsPerSolution = new List<HashSet<int>>();
        _maxSolutions = maxSolutions;
    }

    public void TryAddSolution(List<Item> solution)
    {
        if (!solution.Any())
        {
            Console.WriteLine("Empty solution, skipping.");
            return;
        }

        double score = solution.Sum(item => item.Score);
        double cost = solution.Sum(item => item.AveragePricePerAdult);

        // Diversity Constraint: Ensure solution is not too similar
        bool isUnique = true;
        foreach (var existingItems in _usedItemsPerSolution)
        {
            var intersection = existingItems.Intersect(solution.Select(i => i.Id)).Count();
            if (intersection > solution.Count / 2)
            {
                isUnique = false;
                Console.WriteLine($"Solution too similar to existing, skipping. Score={score}, Cost={cost}");
                break;
            }
        }

        if (isUnique)
        {
            _topSolutions.Add((solution, score));
            _usedItemsPerSolution.Add(new HashSet<int>(solution.Select(i => i.Id)));

            // Remove weakest solution if over limit
            if (_topSolutions.Count > _maxSolutions)
            {
                var minIndex = _topSolutions.IndexOf(_topSolutions.OrderBy(s => s.Score).First());
                Console.WriteLine($"Removing weakest solution at index {minIndex}, Score={_topSolutions[minIndex].Score}");
                _topSolutions.RemoveAt(minIndex);
                _usedItemsPerSolution.RemoveAt(minIndex);
            }

            Console.WriteLine($"Added Solution: Score={score}, Cost={cost}, Items={string.Join(", ", solution.Select(i => i.Name))}");
        }
    }

    public List<List<Item>> GetTopSolutions()
    {
        var solutions = _topSolutions
            .OrderByDescending(s => s.Score)
            .Select(s => s.Solution)
            .ToList();
        Console.WriteLine($"Returning {solutions.Count} top solutions.");
        return solutions;
    }
}