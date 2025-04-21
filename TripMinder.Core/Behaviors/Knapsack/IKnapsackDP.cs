namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackDP
{
    (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds)
        CalculateDP(int budget,
            List<Item> items,
            IKnapsackConstraints constraints);
}