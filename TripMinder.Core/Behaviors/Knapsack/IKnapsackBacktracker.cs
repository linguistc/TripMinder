namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackBacktracker
{
    void BacktrackAllSolutions(KnapsackState state);
    List<Item> BacktrackSingleSolution(KnapsackState state);
    List<List<Item>> BacktrackTopSolutions(KnapsackState state, int maxSolutions = 10);
}