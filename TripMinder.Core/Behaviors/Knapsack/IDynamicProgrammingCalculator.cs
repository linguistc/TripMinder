namespace TripMinder.Core.Behaviors.Knapsack;

public interface IDynamicProgrammingCalculator
{
    (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(int budget, List<Item> items, int maxR = 3, int maxA = 1, int maxE = 3, int maxT = 3, List<Item> baselineItems = null);
}