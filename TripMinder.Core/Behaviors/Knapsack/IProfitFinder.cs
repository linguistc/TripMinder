namespace TripMinder.Core.Behaviors.Knapsack;

public interface IProfitFinder
{
    (float maxProfit, int r, int a, int e, int t) FindMaxProfit(float[,,,,] dp, int budget);
}