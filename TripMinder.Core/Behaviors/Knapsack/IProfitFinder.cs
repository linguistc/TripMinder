using TripMinder.Core.Behaviors.Shared;
namespace TripMinder.Core.Behaviors.Knapsack;

public interface IProfitFinder
{
    (float maxProfit, int usedBudget, int r, int a, int e, int t) FindMaxProfit(
        float[,,,,] dp,
        int budget,
        IKnapsackConstraints constraints,
        bool requireExact = false);
}