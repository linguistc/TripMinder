namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackSolver : IKnapsackSolver
{
    private readonly IKnapsackDP _dpCalculator;
    private readonly IKnapsackBacktracker _backtracker;
    private readonly IProfitFinder _profitFinder;

    public KnapsackSolver(IKnapsackDP dpCalculator, IKnapsackBacktracker backtracker, IProfitFinder profitFinder)
    {
        this._dpCalculator = dpCalculator;
        this._backtracker = backtracker;
        this._profitFinder = profitFinder;
    }

    public (float maxProfit, List<Item> selectedItems) GetMaxProfit(int budget, List<Item> items, IKnapsackConstraints constraints)
    {
        var (dp, decision) = this._dpCalculator.CalculateDP(budget, items);
        var (maxProfit, finalR, finalA, finalE, finalT) = this._profitFinder.FindMaxProfit(dp, budget);
        var state = new KnapsackState(budget, 0, 0, 0, 0, items.Count - 1, items, decision, new List<Item>()); // نبدأ من 0
        var selectedItems = this._backtracker.BacktrackSingleSolution(state);

        Console.WriteLine($"Selected Items Count: {selectedItems.Count}, Total Cost: {selectedItems.Sum(i => i.AveragePricePerAdult)}");
        return (maxProfit, selectedItems);
    }

    public (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(int budget, List<Item> items, IKnapsackConstraints constraints)
    {
        var (dp, decision) = this._dpCalculator.CalculateDP(budget, items);
        var (maxProfit, finalR, finalA, finalE, finalT) = this._profitFinder.FindMaxProfit(dp, budget);
        var optimizer = new SolutionOptimizer(10);
        var state = new KnapsackState(budget, 0, 0, 0, 0, items.Count - 1, items, decision, new List<Item>(), optimizer); // نبدأ من 0
        this._backtracker.BacktrackAllSolutions(state);

        var allSolutions = optimizer.GetTopSolutions();
        Console.WriteLine($"Total Solutions Found: {allSolutions.Count}");
        return (maxProfit, allSolutions);
    }
}