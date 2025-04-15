namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackSolver : IKnapsackSolver
{
    private readonly IKnapsackDP _dpCalculator;
    private readonly IKnapsackBacktracker _backtracker;
    private readonly IProfitFinder _profitFinder;

    public KnapsackSolver(IKnapsackDP dpCalculator, IKnapsackBacktracker backtracker, IProfitFinder profitFinder)
    {
        _dpCalculator = dpCalculator;
        _backtracker = backtracker;
        _profitFinder = profitFinder;
    }

    public (float maxProfit, List<Item> selectedItems) GetMaxProfit(int budget, List<Item> items, IKnapsackConstraints constraints, (int a, int f, int e, int t)? priorities = null)
    {
        var (dp, decision) = _dpCalculator.CalculateDP(budget, items);
        var (maxProfit, finalR, finalA, finalE, finalT) = _profitFinder.FindMaxProfit(dp, budget);
        // تمرير الـ priorities مباشرة للـ KnapsackState
        var state = new KnapsackState(budget, 0, 0, 0, 0, items.Count - 1, items, decision, new List<Item>(), null, priorities);
        var selectedItems = _backtracker.BacktrackSingleSolution(state);

        Console.WriteLine($"Selected Items Count: {selectedItems.Count}, Total Cost: {selectedItems.Sum(i => i.AveragePricePerAdult)}");
        return (maxProfit, selectedItems);
    }

    public (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(int budget, List<Item> items, IKnapsackConstraints constraints, (int a, int f, int e, int t)? priorities = null)
    {
        var (dp, decision) = _dpCalculator.CalculateDP(budget, items);
        var (maxProfit, finalR, finalA, finalE, finalT) = _profitFinder.FindMaxProfit(dp, budget);
        var optimizer = new SolutionOptimizer(10);
        // تمرير الـ priorities مباشرة للـ KnapsackState
        var state = new KnapsackState(budget, 0, 0, 0, 0, items.Count - 1, items, decision, new List<Item>(), optimizer, priorities);
        _backtracker.BacktrackAllSolutions(state);

        var allSolutions = optimizer.GetTopSolutions();
        Console.WriteLine($"Total Solutions Found: {allSolutions.Count}");
        return (maxProfit, allSolutions);
    }
}