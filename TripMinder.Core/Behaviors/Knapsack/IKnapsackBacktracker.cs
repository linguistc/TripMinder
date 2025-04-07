namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackBacktracker
{
    void BacktrackAllSolutions(int w, int r, int a, int e, int t, int index,
        List<Item> items, bool[,,,,,] decision, List<Item> currentSelection, SolutionOptimizer optimizer);
    List<Item> BacktrackSingleSolution(int budget, int r, int a, int e, int t, List<Item> items, bool[,,,,,] decision);
}