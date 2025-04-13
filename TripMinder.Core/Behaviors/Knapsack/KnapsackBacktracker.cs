namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackBacktracker : IKnapsackBacktracker
{
    public void BacktrackAllSolutions(KnapsackState state)
    {
        var stack = new Stack<KnapsackState>();
        stack.Push(state);
        int maxR = state.Decision.GetLength(1) - 1;
        int maxA = state.Decision.GetLength(2) - 1;
        int maxE = state.Decision.GetLength(3) - 1;
        int maxT = state.Decision.GetLength(4) - 1;

        var requiredTypes = new HashSet<ItemType>();
        if (state.Priorities.HasValue)
        {
            if (state.Priorities.Value.a > 0) requiredTypes.Add(ItemType.Accommodation);
            if (state.Priorities.Value.f > 0) requiredTypes.Add(ItemType.Restaurant);
            if (state.Priorities.Value.e > 0) requiredTypes.Add(ItemType.Entertainment);
            if (state.Priorities.Value.t > 0) requiredTypes.Add(ItemType.TourismArea);
        }

        while (stack.Count > 0)
        {
            var currentState = stack.Pop();

            if (currentState.Index < 0)
            {
                var selectedTypes = currentState.CurrentSelection.Select(i => i.PlaceType).ToHashSet();
                if (requiredTypes.All(t => selectedTypes.Contains(t)) || currentState.Budget <= 0)
                {
                    currentState.Optimizer?.TryAddSolution(new List<Item>(currentState.CurrentSelection));
                }
                continue;
            }

            var selectedItem = currentState.Items[currentState.Index];
            int newBudget = currentState.Budget - (int)selectedItem.AveragePricePerAdult;

            stack.Push(currentState with { Index = currentState.Index - 1 });

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
        var currentR = 0;
        var currentA = 0;
        var currentE = 0;
        var currentT = 0;
        var usedItemIds = new HashSet<int>();
        int maxR = state.Decision.GetLength(1) - 1;
        int maxA = state.Decision.GetLength(2) - 1;
        int maxE = state.Decision.GetLength(3) - 1;
        int maxT = state.Decision.GetLength(4) - 1;
        
        
        var priorityOrder = new List<(ItemType Type, int Priority)>();
        if (state.Priorities.HasValue)
        {
            if (state.Priorities.Value.a > 0) priorityOrder.Add((ItemType.Accommodation, state.Priorities.Value.a));
            if (state.Priorities.Value.f > 0) priorityOrder.Add((ItemType.Restaurant, state.Priorities.Value.f));
            if (state.Priorities.Value.e > 0) priorityOrder.Add((ItemType.Entertainment, state.Priorities.Value.e));
            if (state.Priorities.Value.t > 0) priorityOrder.Add((ItemType.TourismArea, state.Priorities.Value.t));
        }
        priorityOrder = priorityOrder.OrderByDescending(p => p.Priority).ToList();

        // المرحلة الأولى: ضمان تغطية الـ Interests
        foreach (var (type, priority) in priorityOrder)
        {
            var availableItems = state.Items
                .Where(i => i.PlaceType == type && i.AveragePricePerAdult <= currentBudget && !usedItemIds.Contains(i.Id))
                .OrderByDescending(i => i.Score) // ترتيب حسب السكور بدل Score/Price
                .ToList();
            if (availableItems.Any())
            {
                var bestItem = availableItems.First();
                Console.WriteLine($"Selected Item (Priority Phase): {bestItem.Name}, Type: {bestItem.PlaceType}, Price: {bestItem.AveragePricePerAdult}, Score: {bestItem.Score}");
                selectedItems.Add(bestItem);
                usedItemIds.Add(bestItem.Id);
                currentBudget -= (int)bestItem.AveragePricePerAdult;
                if (bestItem.PlaceType == ItemType.Restaurant) currentR++;
                if (bestItem.PlaceType == ItemType.Accommodation) currentA++;
                if (bestItem.PlaceType == ItemType.Entertainment) currentE++;
                if (bestItem.PlaceType == ItemType.TourismArea) currentT++;
            }
            else
            {
                Console.WriteLine($"No available items for type {type} in Priority Phase");
            }
        }

        // المرحلة الثانية: استخدام الـ DP لاستغلال الـ Budget المتبقي
        while (currentBudget > 0)
        {
            var availableItems = state.Items
                .Where(i => i.AveragePricePerAdult <= currentBudget && !usedItemIds.Contains(i.Id))
                .Where(i => (i.PlaceType == ItemType.Restaurant && currentR < maxR) ||
                            (i.PlaceType == ItemType.Accommodation && currentA < maxA) ||
                            (i.PlaceType == ItemType.Entertainment && currentE < maxE) ||
                            (i.PlaceType == ItemType.TourismArea && currentT < maxT))
                .OrderByDescending(i => i.Score)
                .ToList();

            if (!availableItems.Any())
                break;

            var bestItem = availableItems.First();
            Console.WriteLine($"Selected Item (Budget Phase): {bestItem.Name}, Type: {bestItem.PlaceType}, Price: {bestItem.AveragePricePerAdult}, Score: {bestItem.Score}");
            
            selectedItems.Add(bestItem);
            usedItemIds.Add(bestItem.Id);
            currentBudget -= (int)bestItem.AveragePricePerAdult;
            if (bestItem.PlaceType == ItemType.Restaurant) currentR++;
            if (bestItem.PlaceType == ItemType.Accommodation) currentA++;
            if (bestItem.PlaceType == ItemType.Entertainment) currentE++;
            if (bestItem.PlaceType == ItemType.TourismArea) currentT++;
        }        
        return selectedItems;
    }
    
    
    /*
    public List<Item> BacktrackSingleSolution(KnapsackState state)
    {
        var selectedItems = new List<Item>();
        var currentBudget = state.Budget;
        var currentR = 0;
        var currentA = 0;
        var currentE = 0;
        var currentT = 0;
        var usedItemIds = new HashSet<int>();
        int maxR = state.Items.Any(i => i.PlaceType == ItemType.Restaurant) ? 4 : 0;
        int maxA = state.Items.Any(i => i.PlaceType == ItemType.Accommodation) ? 1 : 0;
        int maxE = state.Items.Any(i => i.PlaceType == ItemType.Entertainment) ? 2 : 0;
        int maxT = state.Items.Any(i => i.PlaceType == ItemType.TourismArea) ? 1 : 0;

        // ترتيب الأنواع بناءً على الأولويات
        var priorityOrder = new List<(ItemType Type, int Priority)>();
        if (state.Priorities.HasValue)
        {
            if (state.Priorities.Value.a > 0) priorityOrder.Add((ItemType.Accommodation, state.Priorities.Value.a));
            if (state.Priorities.Value.f > 0) priorityOrder.Add((ItemType.Restaurant, state.Priorities.Value.f));
            if (state.Priorities.Value.e > 0) priorityOrder.Add((ItemType.Entertainment, state.Priorities.Value.e));
            if (state.Priorities.Value.t > 0) priorityOrder.Add((ItemType.TourismArea, state.Priorities.Value.t));
        }
        priorityOrder = priorityOrder.OrderByDescending(p => p.Priority).ToList();

        // المرحلة الأولى: اختيار عنصر واحد من كل نوع لو متاح
        foreach (var (type, _) in priorityOrder)
        {
            var availableItems = state.Items.Where(i => i.PlaceType == type && i.AveragePricePerAdult <= currentBudget && !usedItemIds.Contains(i.Id))
                .OrderByDescending(i => i.Score / i.AveragePricePerAdult).ToList();
            if (availableItems.Any()) // لو فيه عناصر متاحة، نختار أفضل واحد
            {
                var bestItem = availableItems.First();
                selectedItems.Add(bestItem);
                usedItemIds.Add(bestItem.Id);
                currentBudget -= (int)bestItem.AveragePricePerAdult;
                if (bestItem.PlaceType == ItemType.Restaurant) currentR++;
                if (bestItem.PlaceType == ItemType.Accommodation) currentA++;
                if (bestItem.PlaceType == ItemType.Entertainment) currentE++;
                if (bestItem.PlaceType == ItemType.TourismArea) currentT++;
            }
        }

        // المرحلة الثانية: استغلال الـ Budget المتبقي
        while (currentBudget > 0)
        {
            bool added = false;
            foreach (var (type, _) in priorityOrder)
            {
                var availableItems = state.Items.Where(i => i.PlaceType == type && i.AveragePricePerAdult <= currentBudget && !usedItemIds.Contains(i.Id))
                    .OrderByDescending(i => i.Score / i.AveragePricePerAdult).ToList();
                if (availableItems.Any() && 
                    ((type == ItemType.Restaurant && currentR < maxR) || 
                     (type == ItemType.Accommodation && currentA < maxA) || 
                     (type == ItemType.Entertainment && currentE < maxE) || 
                     (type == ItemType.TourismArea && currentT < maxT)))
                {
                    var bestItem = availableItems.First();
                    selectedItems.Add(bestItem);
                    usedItemIds.Add(bestItem.Id);
                    currentBudget -= (int)bestItem.AveragePricePerAdult;
                    if (bestItem.PlaceType == ItemType.Restaurant) currentR++;
                    if (bestItem.PlaceType == ItemType.Accommodation) currentA++;
                    if (bestItem.PlaceType == ItemType.Entertainment) currentE++;
                    if (bestItem.PlaceType == ItemType.TourismArea) currentT++;
                    added = true;
                    break;
                }
            }
            if (!added) break; // لو ما فيش حاجة جديدة تضاف، نوقف
        }

        return selectedItems;
    }
    */
}