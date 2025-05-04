namespace TripMinder.Core.Behaviors.Knapsack;

using System.Collections.Generic;

public interface IKnapsackBacktracker
{
    void BacktrackAllSolutions(KnapsackState state, List<DpItem> items, bool[,,,,,] decision);
    List<DpItem> BacktrackSingleSolution(KnapsackState state, List<DpItem> items, bool[,,,,,] decision);
    List<List<DpItem>> BacktrackTopSolutions(KnapsackState state, List<DpItem> items, bool[,,,,,] decision, int maxSolutions = 10);
}