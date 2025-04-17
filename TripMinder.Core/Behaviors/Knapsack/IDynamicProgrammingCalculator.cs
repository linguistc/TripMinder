namespace TripMinder.Core.Behaviors.Knapsack;

public interface IDynamicProgrammingCalculator
{
    (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(int budget, List<Item> items, int maxR, int maxA, int maxE, int maxT);
}