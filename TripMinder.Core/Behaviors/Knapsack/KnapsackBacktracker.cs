namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackBacktracker : IKnapsackBacktracker
{
    private readonly List<Item> _items;
    private readonly bool[,,,,,] _decision;
    private readonly IItemFetcher _itemFetcher;

    public KnapsackBacktracker()
    {
        
    }
    public void BacktrackAllSolutions(KnapsackState state)
    {
        if (state.Budget <= 0 || state.Index < 0 || 
            (state.Restaurants == 0 && state.Accommodations == 0 && state.Entertainments == 0 && state.TourismAreas == 0))
        {
            state.Optimizer?.TryAddSolution(new List<Item>(state.CurrentSelection));
            return;
        }
        if (state.Decision[state.Budget, state.Restaurants, state.Accommodations, state.Entertainments, state.TourismAreas, state.Index])
        {
            var selectedItem = state.Items[state.Index];
            state.CurrentSelection.Add(selectedItem);
            var newState = state with
            {
                Budget = state.Budget - (int)selectedItem.AveragePricePerAdult,
                Restaurants = state.Restaurants - (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0),
                Accommodations = state.Accommodations - (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0),
                Entertainments = state.Entertainments - (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0),
                TourismAreas = state.TourismAreas - (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0),
                Index = state.Index - 1
            };
            BacktrackAllSolutions(newState);
            state.CurrentSelection.RemoveAt(state.CurrentSelection.Count - 1);
        }

        BacktrackAllSolutions(state with { Index = state.Index - 1 });

    }

    public List<Item> BacktrackSingleSolution(KnapsackState state)
    {
        var selectedItems = new List<Item>();
        var currentState = state;

        for (int i = state.Items.Count - 1; i >= 0 && 
                                            (currentState.Restaurants > 0 || currentState.Accommodations > 0 || 
                                             currentState.Entertainments > 0 || currentState.TourismAreas > 0); i--)
        {
            if (currentState.Decision[currentState.Budget, currentState.Restaurants, currentState.Accommodations, 
                    currentState.Entertainments, currentState.TourismAreas, i])
            {
                var selectedItem = currentState.Items[i];
                selectedItems.Add(selectedItem);
                currentState = currentState with
                {
                    Budget = currentState.Budget - (int)selectedItem.AveragePricePerAdult,
                    Restaurants = currentState.Restaurants - (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0),
                    Accommodations = currentState.Accommodations - (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0),
                    Entertainments = currentState.Entertainments - (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0),
                    TourismAreas = currentState.TourismAreas - (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0)
                };
            }
        }

        return selectedItems;
    }
    
    public void BacktrackAllSolutions(int w, int r, int a, int e, int t, int index, List<Item> items, bool[,,,,,] decision, 
        List<Item> currentSelection, SolutionOptimizer optimizer)
    {
        BacktrackAllSolutions(new KnapsackState(w, r, a, e, t, index, items, decision, currentSelection, optimizer));
    }

    public List<Item> BacktrackSingleSolution(int budget, int r, int a, int e, int t, List<Item> items, bool[,,,,,] decision)
    {
        return BacktrackSingleSolution(new KnapsackState(budget, r, a, e, t, items.Count - 1, items, decision, new List<Item>()));
    }
}