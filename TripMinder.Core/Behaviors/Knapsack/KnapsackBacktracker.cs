namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackBacktracker
{
    // دالة لاسترجاع جميع الحلول الممكنة
    public void BacktrackAllSolutions(
        int w, int r, int a, int e, int t, int index,
        List<Item> items, bool[,,,,,] decision,
        List<Item> currentSelection, SolutionOptimizer optimizer)
    {
        if (w <= 0 || index < 0 || (r == 0 && a == 0 && e == 0 && t == 0))
        {
            optimizer.TryAddSolution(new List<Item>(currentSelection));
            return;
        }

        if (decision[w, r, a, e, t, index])
        {
            var selectedItem = items[index];
            currentSelection.Add(selectedItem);

            int newW = w - (int)selectedItem.AveragePricePerAdult;
            int newR = r - (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0);
            int newA = a - (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0);
            int newE = e - (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0);
            int newT = t - (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0);

            BacktrackAllSolutions(newW, newR, newA, newE, newT, index - 1, items, decision, currentSelection, optimizer);

            currentSelection.RemoveAt(currentSelection.Count - 1);
        }

        BacktrackAllSolutions(w, r, a, e, t, index - 1, items, decision, currentSelection, optimizer);
    }

}