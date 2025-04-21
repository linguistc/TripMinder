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
        var solutions = new List<(List<Item> Solution, double Score)>();
        var usedItemsPerSolution = new List<HashSet<string>>();
        var originalScores = state.Items.ToDictionary(i => i.GlobalId, i => i.Score);

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

        // Log decision array for debugging
        Console.WriteLine($"Decision at [{state.Budget},{state.Restaurants},{state.Accommodations},{state.Entertainments},{state.TourismAreas}]: {string.Join(", ", Enumerable.Range(0, state.Items.Count).Select(i => $"Item {state.Items[i].Name}: {(state.Decision[state.Budget, state.Restaurants, state.Accommodations, state.Entertainments, state.TourismAreas, i] ? "True" : "False")}"))}");

        while (stack.Count > 0)
        {
            var (currentState, currentUsedItems, currentScore) = stack.Pop();

            if (currentState.Index < 0)
            {
                var selectedTypes = currentState.CurrentSelection.Select(i => i.PlaceType).ToHashSet();
                // Accept solution if it contains at least one required type or budget is exhausted
                if ((selectedTypes.Intersect(requiredTypes).Any() || currentState.Budget <= 0) && currentState.CurrentSelection.Any())
                {
                    var solution = new List<Item>(currentState.CurrentSelection);
                    double score = currentScore;

                    bool isUnique = true;
                    foreach (var existingSolution in usedItemsPerSolution)
                    {
                        var intersection = existingSolution.Intersect(solution.Select(i => i.GlobalId)).Count();
                        // Relax uniqueness to allow more solutions
                        if (intersection >= solution.Count * 0.75) // Changed from solution.Count / 2 to 0.75
                        {
                            isUnique = false;
                            Console.WriteLine($"Solution rejected (not unique): Items={string.Join(", ", solution.Select(i => i.Name))}, Intersection={intersection}");
                            break;
                        }
                    }

                    if (isUnique)
                    {
                        solutions.Add((solution, score));
                        usedItemsPerSolution.Add(new HashSet<string>(solution.Select(i => i.GlobalId)));
                        Console.WriteLine($"Solution added: Items={string.Join(", ", solution.Select(i => i.Name))}, Score={score}, Budget={currentState.Budget}, Types={string.Join(", ", selectedTypes)}");

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

            var selectedItem = currentState.Items[currentState.Index];
            int newBudget = currentState.Budget - (int)selectedItem.AveragePricePerAdult;

            // Push state without selecting the item
            stack.Push((currentState with { Index = currentState.Index - 1 }, currentUsedItems, currentScore));

            int newR = currentState.Restaurants - (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0);
            int newA = currentState.Accommodations - (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0);
            int newE = currentState.Entertainments - (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0);
            int newT = currentState.TourismAreas - (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0);

            // Check if item can be included based on decision and constraints
            if (newR >= 0 && newA >= 0 && newE >= 0 && newT >= 0 && newBudget >= 0 &&
                currentState.Decision[currentState.Budget, currentState.Restaurants, currentState.Accommodations, currentState.Entertainments, currentState.TourismAreas, currentState.Index])
            {
                int usageCount = usedItemsPerSolution.Count(s => s.Contains(selectedItem.GlobalId));
                // Relax usage count further
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
                    stack.Push((newState, newUsedItems, currentScore + selectedItem.Score));
                    Console.WriteLine($"Pushing state: Item={selectedItem.Name}, GlobalId={selectedItem.GlobalId}, New Budget={newBudget}, New Restaurants={newR}, Score={currentScore + selectedItem.Score}");
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

            // Reduce score to encourage diversity
            selectedItem.Score = selectedItem.Score * 0.9f; // Changed from 0.7f to 0.9f for less aggressive reduction
        }

        foreach (var item in state.Items)
        {
            item.Score = originalScores[item.GlobalId];
        }

        var finalSolutions = solutions
            .OrderByDescending(s => s.Score)
            .Select(s => s.Solution)
            .Take(maxSolutions)
            .ToList();

        Console.WriteLine($"BacktrackTopSolutions Complete: Found {finalSolutions.Count} solutions");
        return finalSolutions;
    }

    public List<Item> BacktrackSingleSolution(KnapsackState state)
    {
        var selectedItems = new List<Item>();
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
            var item = state.Items[i];
            Console.WriteLine($"Checking Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Decision[{currentW},{currentR},{currentA},{currentE},{currentT},{i}]={(state.Decision[currentW, currentR, currentA, currentE, currentT, i] ? "True" : "False")}");

            if (currentW < 0 || currentR < 0 || currentA < 0 || currentE < 0 || currentT < 0)
            {
                Console.WriteLine($"Invalid state reached: Budget={currentW}, Restaurants={currentR}, Accommodations={currentA}, Entertainments={currentE}, TourismAreas={currentT}");
                break;
            }

            if (state.Decision[currentW, currentR, currentA, currentE, currentT, i])
            {
                if (!usedItemIds.Contains(item.GlobalId))
                {
                    selectedItems.Add(item);
                    usedItemIds.Add(item.GlobalId);
                    currentW -= (int)item.AveragePricePerAdult;
                    if (item.PlaceType == ItemType.Restaurant) currentR--;
                    if (item.PlaceType == ItemType.Accommodation) currentA--;
                    if (item.PlaceType == ItemType.Entertainment) currentE--;
                    if (item.PlaceType == ItemType.TourismArea) currentT--;
                    Console.WriteLine($"Selected Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}, New Budget={currentW}, New Restaurants={currentR}");
                }
            }
        }

        var selectedTypes = selectedItems.Select(i => i.PlaceType).ToHashSet();
        if (!requiredTypes.All(t => selectedTypes.Contains(t)))
        {
            Console.WriteLine("Warning: Solution does not meet all priority requirements.");
        }

        Console.WriteLine($"Backtracking Complete: Selected {selectedItems.Count(i => i.PlaceType == ItemType.Restaurant)} Restaurants, Total Items={selectedItems.Count}");
        return selectedItems;
    }
}