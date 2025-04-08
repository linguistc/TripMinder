namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackBacktracker : IKnapsackBacktracker
{
    public void BacktrackAllSolutions(KnapsackState state)
    {
        var stack = new Stack<KnapsackState>();
        stack.Push(state);
        int maxR = 4, maxA = 1, maxE = 2, maxT = 1;

        while (stack.Count > 0)
        {
            var currentState = stack.Pop();

            if (currentState.Index < 0)
            {
                if (currentState.Accommodations >= 1)
                {
                    currentState.Optimizer?.TryAddSolution(new List<Item>(currentState.CurrentSelection));
                }
                continue;
            }

            var selectedItem = currentState.Items[currentState.Index];
            int newBudget = currentState.Budget - (int)selectedItem.AveragePricePerAdult;

            // بدون الـ Item
            stack.Push(currentState with { Index = currentState.Index - 1 });

            // مع الـ Item
            int newR = currentState.Restaurants + (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0);
            int newA = currentState.Accommodations + (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0);
            int newE = currentState.Entertainments + (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0);
            int newT = currentState.TourismAreas + (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0);

            if (newR <= maxR && newA <= maxA && newE <= maxE && newT <= maxT &&
                newBudget >= 0 &&
                !currentState.CurrentSelection.Any(i => i.Id == selectedItem.Id))
            {
                var newSelection = new List<Item>(currentState.CurrentSelection) { selectedItem };
                var newState = currentState with
                {
                    Budget = newBudget,
                    Restaurants = newR,
                    Accommodations = newA,
                    Entertainments = newE,
                    TourismAreas = newT,
                    Index = currentState.Index - 1,
                    CurrentSelection = newSelection
                };
                stack.Push(newState);
            }
        }
    }

    public List<Item> BacktrackSingleSolution(KnapsackState state)
    {
        var selectedItems = new List<Item>();
        var currentBudget = state.Budget;
        var currentR = 0; // نبدأ من 0 ونبني الحل
        var currentA = 0;
        var currentE = 0;
        var currentT = 0;
        var usedItemIds = new HashSet<int>();
        int maxR = 4, maxA = 1, maxE = 2, maxT = 1;

        for (int i = 0; i < state.Items.Count; i++) // من البداية للنهاية
        {
            if (currentBudget <= 0) break;

            if (currentR <= maxR && currentA <= maxA && currentE <= maxE && currentT <= maxT &&
                state.Decision[currentBudget, currentR, currentA, currentE, currentT, i])
            {
                var selectedItem = state.Items[i];
                if (!usedItemIds.Contains(selectedItem.Id))
                {
                    int cost = (int)selectedItem.AveragePricePerAdult;
                    if (cost <= currentBudget)
                    {
                        selectedItems.Add(selectedItem);
                        usedItemIds.Add(selectedItem.Id);
                        currentBudget -= cost;
                        currentR += selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0;
                        currentA += selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0;
                        currentE += selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0;
                        currentT += selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0;
                    }
                }
            }
        }

        if (!selectedItems.Any(i => i.PlaceType == ItemType.Accommodation))
        {
            var accommodations = state.Items.Where(i => i.PlaceType == ItemType.Accommodation && i.AveragePricePerAdult <= state.Budget).ToList();
            if (accommodations.Any())
            {
                var bestAccommodation = accommodations.OrderByDescending(i => i.Score / i.AveragePricePerAdult).First();
                selectedItems.Insert(0, bestAccommodation);
            }
        }

        return selectedItems;
    }
}