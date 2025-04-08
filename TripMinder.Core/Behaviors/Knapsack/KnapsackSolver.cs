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
        if (budget < 0) throw new ArgumentException("Budget cannot be negative.");
        if (items == null || !items.Any()) throw new ArgumentException("Items list cannot be null or empty.");
        
        var (dp, decision) = this._dpCalculator.CalculateDP(budget, items);
        var (maxProfit, finalR, finalA, finalE, finalT) = this._profitFinder.FindMaxProfit(dp, budget);
        var state = new KnapsackState(budget, finalR, finalA, finalE, finalT, items.Count - 1, items, decision, new List<Item>());
        var selectedItems = this._backtracker.BacktrackSingleSolution(state);
        return (maxProfit, selectedItems);
    }
    
    public (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(int budget, List<Item> items, IKnapsackConstraints constraints)
    {
        var (dp, decision) = this._dpCalculator.CalculateDP(budget, items);
        var (maxProfit, finalR, finalA, finalE, finalT) = this._profitFinder.FindMaxProfit(dp, budget);
        var optimizer = new SolutionOptimizer(10);
        var state = new KnapsackState(budget, finalR, finalA, finalE, finalT, items.Count - 1, items, decision, new List<Item>(), optimizer);
        this._backtracker.BacktrackAllSolutions(state);
        return (maxProfit, optimizer.GetTopSolutions());
    }
}