namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackBacktracker : IKnapsackBacktracker
{
    public void BacktrackAllSolutions(KnapsackState state)
    {
        var solutions = BacktrackTopSolutions(state);
        if (state.Optimizer != null)
        {
            foreach (var solution in solutions)
            {
                state.Optimizer.TryAddSolution(solution);
            }
        }
    }

    public List<List<Item>> BacktrackTopSolutions(KnapsackState state, int maxSolutions = 10)
    {
        // Check for duplicate IDs
        var duplicateIds = state.Items
            .GroupBy(i => i.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateIds.Any())
        {
            throw new InvalidOperationException($"Duplicate item IDs found: {string.Join(", ", duplicateIds)}");
        }
        
        var solutions = new List<(List<Item> Solution, double Score)>();
        var usedItemsPerSolution = new List<HashSet<int>>();
        var originalScores = state.Items.ToDictionary(i => i.Id, i => i.Score);

        var stack = new Stack<(KnapsackState State, HashSet<int> UsedItems, double CurrentScore)>();
        stack.Push((state with { CurrentSelection = new List<Item>() }, new HashSet<int>(), 0));

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
            var (currentState, currentUsedItems, currentScore) = stack.Pop();

            if (currentState.Index < 0)
            {
                var selectedTypes = currentState.CurrentSelection.Select(i => i.PlaceType).ToHashSet();
                if ((requiredTypes.All(t => selectedTypes.Contains(t)) || currentState.Budget <= 0) && currentState.CurrentSelection.Any())
                {
                    var solution = new List<Item>(currentState.CurrentSelection);
                    double score = currentScore;

                    bool isUnique = true;
                    foreach (var existingSolution in usedItemsPerSolution)
                    {
                        var intersection = existingSolution.Intersect(solution.Select(i => i.Id)).Count();
                        if (intersection > solution.Count / 2)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (isUnique)
                    {
                        solutions.Add((solution, score));
                        usedItemsPerSolution.Add(new HashSet<int>(solution.Select(i => i.Id)));

                        if (solutions.Count > maxSolutions)
                        {
                            var minIndex = solutions.IndexOf(solutions.OrderBy(s => s.Score).First());
                            solutions.RemoveAt(minIndex);
                            usedItemsPerSolution.RemoveAt(minIndex);
                        }
                    }
                }
                continue;
            }

            var selectedItem = currentState.Items[currentState.Index];
            int newBudget = currentState.Budget - (int)selectedItem.AveragePricePerAdult;

            stack.Push((currentState with { Index = currentState.Index - 1 }, currentUsedItems, currentScore));

            int newR = currentState.Restaurants + (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0);
            int newA = currentState.Accommodations + (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0);
            int newE = currentState.Entertainments + (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0);
            int newT = currentState.TourismAreas + (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0);

            if (newR <= maxR && newA <= maxA && newE <= maxE && newT <= maxT && newBudget >= 0)
            {
                int usageCount = usedItemsPerSolution.Count(s => s.Contains(selectedItem.Id));
                if (usageCount < 2)
                {
                    var newSelection = new List<Item>(currentState.CurrentSelection) { selectedItem };
                    var newUsedItems = new HashSet<int>(currentUsedItems) { selectedItem.Id };
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
                    stack.Push((newState, newUsedItems, currentScore + selectedItem.Score));
                }
            }

            selectedItem.Score = selectedItem.Score * 0.7f;
        }

        foreach (var item in state.Items)
        {
            item.Score = originalScores[item.Id];
        }

        return solutions
            .OrderByDescending(s => s.Score)
            .Select(s => s.Solution)
            .Take(maxSolutions)
            .ToList();
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

        foreach (var (type, priority) in priorityOrder)
        {
            var availableItems = state.Items
                .Where(i => i.PlaceType == type && i.AveragePricePerAdult <= currentBudget && !usedItemIds.Contains(i.Id))
                .OrderByDescending(i => i.Score)
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
        }

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
}