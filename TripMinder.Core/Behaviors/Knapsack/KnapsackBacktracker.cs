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
                state.Optimizer.TryAddSolution(solution.Select(d => d.Original).ToList());
            }
        }
    }

    public List<List<DpItem>> BacktrackTopSolutions(KnapsackState state, int maxSolutions = 10)
    {
        var solutions = new List<(List<DpItem> Solution, double Score)>();
        var usedItemsPerSolution = new List<HashSet<string>>();
        var originalScores = state.Items.ToDictionary(i => i.Original.GlobalId, i => i.Profit);

        var stack = new Stack<(KnapsackState State, HashSet<string> UsedItems, double CurrentScore)>();
        stack.Push((state with { CurrentSelection = new List<Item>() }, new HashSet<string>(), 0));

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

        Console.WriteLine($"BacktrackTopSolutions: Required Types={string.Join(", ", requiredTypes)}, Initial Budget={state.Budget}, Restaurants={state.Restaurants}, Accommodations={state.Accommodations}, Entertainments={state.Entertainments}, TourismAreas={state.TourismAreas}, Total Items={state.Items.Count}");

        while (stack.Count > 0)
        {
            var (currentState, currentUsedItems, currentScore) = stack.Pop();

            if (currentState.Index < 0)
            {
                var selectedTypes = currentState.CurrentSelection.Select(i => i.PlaceType).ToHashSet();
                if ((selectedTypes.Intersect(requiredTypes).Any() || currentState.Budget <= 0) && currentState.CurrentSelection.Any())
                {
                    var solution = currentState.CurrentSelection.Select(i => state.Items.First(d => d.Original.GlobalId == i.GlobalId)).ToList();
                    double score = currentScore;

                    bool isUnique = true;
                    foreach (var existingSolution in usedItemsPerSolution)
                    {
                        var intersection = existingSolution.Intersect(solution.Select(d => d.Original.GlobalId)).Count();
                        if (intersection >= solution.Count * 0.75)
                        {
                            isUnique = false;
                            Console.WriteLine($"Solution rejected (not unique): Items={string.Join(", ", solution.Select(d => d.Original.Name))}, Intersection={intersection}");
                            break;
                        }
                    }

                    if (isUnique)
                    {
                        solutions.Add((solution, score));
                        usedItemsPerSolution.Add(new HashSet<string>(solution.Select(d => d.Original.GlobalId)));
                        Console.WriteLine($"Solution added: Items={string.Join(", ", solution.Select(d => d.Original.Name))}, Score={score}, Budget={currentState.Budget}, Types={string.Join(", ", selectedTypes)}");

                        if (solutions.Count > maxSolutions)
                        {
                            var minIndex = solutions.IndexOf(solutions.OrderBy(s => s.Score).First());
                            solutions.RemoveAt(minIndex);
                            usedItemsPerSolution.RemoveAt(minIndex);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Solution rejected: Types={string.Join(", ", selectedTypes)}, Budget={currentState.Budget}, Required={string.Join(", ", requiredTypes)}");
                }
                continue;
            }

            var selectedDpItem = (DpItem)currentState.Items[currentState.Index];
            var selectedItem = selectedDpItem.Original;
            int newBudget = currentState.Budget - selectedDpItem.Weight;

            stack.Push((currentState with { Index = currentState.Index - 1 }, currentUsedItems, currentScore));

            int newR = currentState.Restaurants - (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0);
            int newA = currentState.Accommodations - (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0);
            int newE = currentState.Entertainments - (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0);
            int newT = currentState.TourismAreas - (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0);

            if (newR >= 0 && newA >= 0 && newE >= 0 && newT >= 0 && newBudget >= 0 &&
                currentState.Decision[currentState.Budget, currentState.Restaurants, currentState.Accommodations, currentState.Entertainments, currentState.TourismAreas, currentState.Index])
            {
                int usageCount = usedItemsPerSolution.Count(s => s.Contains(selectedItem.GlobalId));
                int maxUsage = state.Items.Count < 15 ? 4 : 3;
                if (usageCount < maxUsage)
                {
                    var newSelection = new List<Item>(currentState.CurrentSelection) { selectedItem };
                    var newUsedItems = new HashSet<string>(currentUsedItems) { selectedItem.GlobalId };
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
                    stack.Push((newState, newUsedItems, currentScore + selectedDpItem.Profit));
                    Console.WriteLine($"Pushing state: Item={selectedItem.Name}, GlobalId={selectedItem.GlobalId}, New Budget={newBudget}, New Restaurants={newR}, Score={currentScore + selectedDpItem.Profit}");
                }
                else
                {
                    Console.WriteLine($"Item skipped (usage limit): {selectedItem.Name}, GlobalId={selectedItem.GlobalId}, UsageCount={usageCount}");
                }
            }
            else
            {
                Console.WriteLine($"Item skipped: {selectedItem.Name}, GlobalId={selectedItem.GlobalId}, Budget={currentState.Budget}, Restaurants={currentState.Restaurants}, Decision={(currentState.Decision[currentState.Budget, currentState.Restaurants, currentState.Accommodations, currentState.Entertainments, currentState.TourismAreas, currentState.Index] ? "True" : "False")}");
            }

            selectedDpItem.Original.Score = selectedDpItem.Original.Score * 0.9f;
        }

        foreach (var dpItem in state.Items)
        {
            dpItem.Original.Score = originalScores[dpItem.Original.GlobalId];
        }

        var finalSolutions = solutions
            .OrderByDescending(s => s.Score)
            .Select(s => s.Solution)
            .Take(maxSolutions)
            .ToList();

        Console.WriteLine($"BacktrackTopSolutions Complete: Found {finalSolutions.Count} solutions");
        return finalSolutions;
    }

    public List<DpItem> BacktrackSingleSolution(KnapsackState state)
    {
        var selectedItems = new List<DpItem>();
        int currentW = state.Budget;
        int currentR = state.Restaurants;
        int currentA = state.Accommodations;
        int currentE = state.Entertainments;
        int currentT = state.TourismAreas;
        var usedItemIds = new HashSet<string>();

        var requiredTypes = new HashSet<ItemType>();
        if (state.Priorities.HasValue)
        {
            if (state.Priorities.Value.a > 0) requiredTypes.Add(ItemType.Accommodation);
            if (state.Priorities.Value.f > 0) requiredTypes.Add(ItemType.Restaurant);
            if (state.Priorities.Value.e > 0) requiredTypes.Add(ItemType.Entertainment);
            if (state.Priorities.Value.t > 0) requiredTypes.Add(ItemType.TourismArea);
        }

        Console.WriteLine($"Starting Backtracking: Budget={currentW}, Restaurants={currentR}, Accommodations={currentA}, Entertainments={currentE}, TourismAreas={currentT}");

        for (int i = state.Items.Count - 1; i >= 0; i--)
        {
            var dpItem = (DpItem)state.Items[i];
            var item = dpItem.Original;
            if (currentW < 0 || currentR < 0 || currentA < 0 || currentE < 0 || currentT < 0)
            {
                Console.WriteLine($"Invalid state reached: Budget={currentW}, Restaurants={currentR}, Accommodations={currentA}, Entertainments={currentE}, TourismAreas={currentT}");
                break;
            }

            if (state.Decision[currentW, currentR, currentA, currentE, currentT, i] && !usedItemIds.Contains(item.GlobalId))
            {
                if (requiredTypes.Contains(item.PlaceType))
                {
                    selectedItems.Add(dpItem);
                    usedItemIds.Add(item.GlobalId);
                    currentW -= dpItem.Weight;
                    if (item.PlaceType == ItemType.Restaurant) currentR--;
                    if (item.PlaceType == ItemType.Accommodation) currentA--;
                    if (item.PlaceType == ItemType.Entertainment) currentE--;
                    if (item.PlaceType == ItemType.TourismArea) currentT--;
                    Console.WriteLine($"Selected Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {dpItem.Weight}, Profit: {dpItem.Profit}, New Budget={currentW}");
                }
            }
        }

        var selectedTypes = selectedItems.Select(d => d.Original.PlaceType).ToHashSet();
        if (!requiredTypes.All(t => selectedTypes.Contains(t)))
        {
            Console.WriteLine($"Warning: Solution does not meet all priority requirements. Missing types: {string.Join(", ", requiredTypes.Except(selectedTypes))}");
        }

        Console.WriteLine($"Backtracking Complete: Selected {selectedItems.Count} Items");
        return selectedItems;
    }
}