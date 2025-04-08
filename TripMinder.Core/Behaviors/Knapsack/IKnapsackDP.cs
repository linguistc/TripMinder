namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackDP
{
    (float[,,,,] dp, bool[,,,,,] decision) CalculateDP(int budget, List<Item> items);
}