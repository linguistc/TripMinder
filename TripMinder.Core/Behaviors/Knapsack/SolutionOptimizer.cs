namespace TripMinder.Core.Behaviors.Knapsack;

public class SolutionOptimizer
{
    private readonly PriorityQueue<List<Item>, double> _topSolutions;
    private readonly int _maxSolutions;

    public SolutionOptimizer(int maxSolutions = 10)
    {
        _topSolutions = new PriorityQueue<List<Item>, double>();
        _maxSolutions = maxSolutions;
    }

    public void TryAddSolution(List<Item> solution)
    {
        double score = solution.Sum(item => item.Score);

        // لو لسه ممليناش الهيب، ضيف مباشرة
        if (_topSolutions.Count < _maxSolutions)
        {
            _topSolutions.Enqueue(solution, score);
        }
        else
        {
            // لو الحل الحالي أفضل من الأسوأ في الهيب، استبدله
            if (_topSolutions.TryPeek(out _, out double minScore) && score > minScore)
            {
                _topSolutions.Dequeue(); // احذف الأضعف
                _topSolutions.Enqueue(solution, score); // ضيف الجديد
            }
        }
    }

    public List<List<Item>> GetTopSolutions()
    {
        return _topSolutions.UnorderedItems
            .Select(tuple => tuple.Element) // استخرج الحلول
            .OrderByDescending(sol => sol.Sum(item => item.Score)) // رتبهم تنازليًا
            .ToList();
    }
}