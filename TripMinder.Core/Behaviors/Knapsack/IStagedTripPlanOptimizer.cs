namespace TripMinder.Core.Behaviors.Knapsack;

public interface IStagedTripPlanOptimizer
{
    Task<List<Item>> OptimizeStagedAsync(
        List<Item> items,
        List<string> orderedInterests,
        int budget,
        UserDefinedKnapsackConstraints constraints,
        (int a, int f, int e, int t)? priorities = null);
}