using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public interface IGreedyTripSolver
{
    (float maxProfit, List<Item> selectedItems) GetBestPlan(int budget, List<Item> items, IKnapsackConstraints constraints, (int a, int f, int e, int t)? priorities = null);
    (float maxProfit, List<List<Item>> allPlans) GetMultiplePlans(int budget, List<Item> items, IKnapsackConstraints constraints, (int a, int f, int e, int t)? priorities = null);
}