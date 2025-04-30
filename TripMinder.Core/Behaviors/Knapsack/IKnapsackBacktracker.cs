namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackBacktracker
{
    void BacktrackAllSolutions(KnapsackState state);
    List<DpItem> BacktrackSingleSolution(KnapsackState state);
    List<List<DpItem>> BacktrackTopSolutions(KnapsackState state, int maxSolutions = 10);
}