namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackDP
{
    (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(
        int budget, List<DpItem> items, int maxR, int maxA, int maxE, int maxT);
}