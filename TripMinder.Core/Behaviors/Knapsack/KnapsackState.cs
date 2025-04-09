namespace TripMinder.Core.Behaviors.Knapsack;

public record KnapsackState(
    int Budget,
    int Restaurants,
    int Accommodations,
    int Entertainments,
    int TourismAreas,
    int Index,
    List<Item> Items,
    bool[,,,,,] Decision,
    List<Item> CurrentSelection,
    SolutionOptimizer? Optimizer = null,
    (int a, int f, int e, int t)? Priorities = null);