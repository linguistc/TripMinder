using TripMinder.Core.Behaviors.Knapsack;

namespace TripMinder.Core.Behaviors;

public interface IKnapsackSolver
{
    (float maxProfit, List<Item> selectedItems) GetMaxProfit(int budget, List<Item> items);
    (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(int budget, List<Item> items);
}