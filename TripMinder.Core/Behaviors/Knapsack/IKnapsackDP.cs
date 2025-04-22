namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackDP
{
    (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(
        int budget, List<Item> items, int maxR, int maxA, int maxE, int maxT, List<Item> baselineItems = null);
}