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

    public (float maxProfit, List<Item> selectedItems) GetMaxProfit(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null, bool requireExact = false)
    {
// تحويل Items إلى DpItems
        var dpItems = items.Select(i => new DpItem(i)).ToList();

        var (dp, decision, itemIds) = _dpCalculator.Calculate(
            budget, dpItems,
            constraints.MaxRestaurants,
            constraints.MaxAccommodations,
            constraints.MaxEntertainments,
            constraints.MaxTourismAreas);

        var (maxProfit, usedBudget, finalR, finalA, finalE, finalT) =
            _profitFinder.FindMaxProfit(dp, budget, constraints, requireExact);
        Console.WriteLine(
            $"Max Profit: {maxProfit}, Final State: Restaurants={finalR}, Accommodations={finalA}, Entertainments={finalE}, TourismAreas={finalT}, Used Budget={usedBudget}");

        var state = new KnapsackState(
            usedBudget,
            finalR,
            finalA,
            finalE,
            finalT,
            dpItems.Count - 1,
            dpItems,
            decision,
            new List<Item>(),
            null,
            priorities);

        var selectedDpItems = _backtracker.BacktrackSingleSolution(state);
        var selectedItems = selectedDpItems.Select(d => d.Original).ToList();

        Console.WriteLine(
            $"Selected Items Count: {selectedItems.Count}, Total Cost: {selectedItems.Sum(i => i.AveragePricePerAdult)}");
        return (maxProfit, selectedItems);
    }

    public (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(
        int budget,
        List<Item> items,
        IKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null, bool requireExact = false)
    {
        var dpItems = items.Select(i => new DpItem(i)).ToList();

        var (dp, decision, itemIds) = _dpCalculator.Calculate(
            budget, dpItems,
            constraints.MaxRestaurants,
            constraints.MaxAccommodations,
            constraints.MaxEntertainments,
            constraints.MaxTourismAreas);

        var (maxProfit, usedBudget, finalR, finalA, finalE, finalT) =
            _profitFinder.FindMaxProfit(dp, budget, constraints, requireExact);
        Console.WriteLine(
            $"Max Profit: {maxProfit}, Final State: Restaurants={finalR}, Accommodations={finalA}, Entertainments={finalE}, TourismAreas={finalT}, Used Budget={usedBudget}");

        var state = new KnapsackState(usedBudget, finalR, finalA, finalE, finalT, dpItems.Count - 1, dpItems, decision,
            new List<Item>(), null, priorities);
        var allSolutions = _backtracker.BacktrackTopSolutions(state, 10);

        var allSelectedItems = allSolutions.Select(solution => solution.Select(d => d.Original).ToList()).ToList();
        Console.WriteLine($"Total Solutions Found: {allSelectedItems.Count}");
        return (maxProfit, allSelectedItems);
    }
}