using System;
using System.Collections.Generic;
using System.Linq;
using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackBacktracker : IKnapsackBacktracker
{
    public void BacktrackAllSolutions(KnapsackState state, List<DpItem> items, bool[,,,,,] decision)
    {
        var solutions = BacktrackTopSolutions(state, items, decision);
        // Removed Optimizer dependency as it's not part of the new KnapsackState
        Console.WriteLine($"BacktrackAllSolutions: Found {solutions.Count} solutions");
    }

    public List<List<DpItem>> BacktrackTopSolutions(KnapsackState state, List<DpItem> items, bool[,,,,,] decision, int maxSolutions = 10)
    {
        var solutions = new List<(List<DpItem> Solution, double Score)>();
        var usedItemsPerSolution = new List<HashSet<string>>();
        var originalScores = items.ToDictionary(i => i.Original.GlobalId, i => (double)i.Original.Score);
        var modifiedScores = new Dictionary<string, double>();

        var stack = new Stack<(KnapsackState State, HashSet<string> UsedItems, double CurrentScore)>();
        var initialState = state.Clone();
        initialState.SelectedItems = new List<DpItem>();
        stack.Push((initialState, new HashSet<string>(), 0));

        int maxR = decision.GetLength(1) - 1;
        int maxA = decision.GetLength(2) - 1;
        int maxE = decision.GetLength(3) - 1;
        int maxT = decision.GetLength(4) - 1;

        var requiredTypes = new HashSet<ItemType>();
        if (state.Priorities.HasValue)
        {
            if (state.Priorities.Value.a > 0) requiredTypes.Add(ItemType.Accommodation);
            if (state.Priorities.Value.f > 0) requiredTypes.Add(ItemType.Restaurant);
            if (state.Priorities.Value.e > 0) requiredTypes.Add(ItemType.Entertainment);
            if (state.Priorities.Value.t > 0) requiredTypes.Add(ItemType.TourismArea);
        }

        Console.WriteLine($"BacktrackTopSolutions: Required Types={string.Join(", ", requiredTypes)}, Initial Budget={state.RemainingBudget}, Restaurants={state.CategoryCounts.GetValueOrDefault(ItemType.Restaurant, 0)}, Accommodations={state.CategoryCounts.GetValueOrDefault(ItemType.Accommodation, 0)}, Entertainments={state.CategoryCounts.GetValueOrDefault(ItemType.Entertainment, 0)}, TourismAreas={state.CategoryCounts.GetValueOrDefault(ItemType.TourismArea, 0)}, Total Items={items.Count}");

        while (stack.Count > 0)
        {
            var (currentState, currentUsedItems, currentScore) = stack.Pop();

            if (currentState.SelectedItems.Count == items.Count || currentState.RemainingBudget <= 0)
            {
                var selectedTypes = currentState.SelectedItems.Select(i => i.PlaceType).ToHashSet();
                if ((selectedTypes.Intersect(requiredTypes).Any() || currentState.RemainingBudget <= 0) && currentState.SelectedItems.Any())
                {
                    var solution = currentState.SelectedItems.ToList();
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
                        Console.WriteLine($"Solution added: Items={string.Join(", ", solution.Select(d => d.Original.Name))}, Score={score}, Remaining Budget={currentState.RemainingBudget}, Types={string.Join(", ", selectedTypes)}");

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
                    Console.WriteLine($"Solution rejected: Types={string.Join(", ", selectedTypes)}, Remaining Budget={currentState.RemainingBudget}, Required={string.Join(", ", requiredTypes)}");
                }
                continue;
            }

            stack.Push((currentState.Clone(), new HashSet<string>(currentUsedItems), currentScore));

            for (int i = items.Count - 1; i >= 0; i--)
            {
                var selectedDpItem = items[i];
                var selectedItem = selectedDpItem.Original;
                int newBudget = currentState.RemainingBudget - selectedDpItem.Weight;

                int currentR = currentState.CategoryCounts.GetValueOrDefault(ItemType.Restaurant, 0);
                int currentA = currentState.CategoryCounts.GetValueOrDefault(ItemType.Accommodation, 0);
                int currentE = currentState.CategoryCounts.GetValueOrDefault(ItemType.Entertainment, 0);
                int currentT = currentState.CategoryCounts.GetValueOrDefault(ItemType.TourismArea, 0);

                int newR = currentR - (selectedItem.PlaceType == ItemType.Restaurant ? 1 : 0);
                int newA = currentA - (selectedItem.PlaceType == ItemType.Accommodation ? 1 : 0);
                int newE = currentE - (selectedItem.PlaceType == ItemType.Entertainment ? 1 : 0);
                int newT = currentT - (selectedItem.PlaceType == ItemType.TourismArea ? 1 : 0);

                if (newR >= 0 && newA >= 0 && newE >= 0 && newT >= 0 && newBudget >= 0 &&
                    decision[currentState.RemainingBudget, currentR, currentA, currentE, currentT, i])
                {
                    int usageCount = usedItemsPerSolution.Count(s => s.Contains(selectedItem.GlobalId));
                    int maxUsage = items.Count < 15 ? 4 : 3;
                    if (usageCount < maxUsage)
                    {
                        var newState = currentState.Clone();
                        newState.SelectedItems.Add(selectedDpItem);
                        newState.RemainingBudget = newBudget;
                        newState.CategoryCounts[ItemType.Restaurant] = newR;
                        newState.CategoryCounts[ItemType.Accommodation] = newA;
                        newState.CategoryCounts[ItemType.Entertainment] = newE;
                        newState.CategoryCounts[ItemType.TourismArea] = newT;

                        var newUsedItems = new HashSet<string>(currentUsedItems) { selectedItem.GlobalId };
                        double modifiedScore = selectedDpItem.Profit * (modifiedScores.ContainsKey(selectedItem.GlobalId) ? modifiedScores[selectedItem.GlobalId] : 1.0);
                        modifiedScores[selectedItem.GlobalId] = modifiedScores.GetValueOrDefault(selectedItem.GlobalId, 1.0) * 0.9;

                        stack.Push((newState, newUsedItems, currentScore + modifiedScore));
                        Console.WriteLine($"Pushing state: Item={selectedItem.Name}, GlobalId={selectedItem.GlobalId}, New Budget={newBudget}, New Restaurants={newR}, Score={currentScore + modifiedScore}");
                    }
                    else
                    {
                        Console.WriteLine($"Item skipped (usage limit): {selectedItem.Name}, GlobalId={selectedItem.GlobalId}, UsageCount={usageCount}");
                    }
                }
                else
                {
                    Console.WriteLine($"Item skipped: {selectedItem.Name}, GlobalId={selectedItem.GlobalId}, Budget={currentState.RemainingBudget}, Restaurants={currentR}, Decision={(decision[currentState.RemainingBudget, currentR, currentA, currentE, currentT, i] ? "True" : "False")}");
                }
            }
        }

        foreach (var item in items)
        {
            if (modifiedScores.ContainsKey(item.Original.GlobalId))
                item.Original.Score = (float)originalScores[item.Original.GlobalId];
        }

        var finalSolutions = solutions
            .OrderByDescending(s => s.Score)
            .Select(s => s.Solution)
            .Take(maxSolutions)
            .ToList();

        Console.WriteLine($"BacktrackTopSolutions Complete: Found {finalSolutions.Count} solutions");
        return finalSolutions;
    }

    public List<DpItem> BacktrackSingleSolution(KnapsackState state, List<DpItem> items, bool[,,,,,] decision)
    {
        var selectedItems = new List<DpItem>();
        int currentW = state.RemainingBudget;
        int currentR = state.CategoryCounts.GetValueOrDefault(ItemType.Restaurant, 0);
        int currentA = state.CategoryCounts.GetValueOrDefault(ItemType.Accommodation, 0);
        int currentE = state.CategoryCounts.GetValueOrDefault(ItemType.Entertainment, 0);
        int currentT = state.CategoryCounts.GetValueOrDefault(ItemType.TourismArea, 0);
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

        for (int i = items.Count - 1; i >= 0; i--)
        {
            var dpItem = items[i];
            var item = dpItem.Original;
            if (currentW < 0 || currentR < 0 || currentA < 0 || currentE < 0 || currentT < 0)
            {
                Console.WriteLine($"Invalid state reached: Budget={currentW}, Restaurants={currentR}, Accommodations={currentA}, Entertainments={currentE}, TourismAreas={currentT}");
                break;
            }

            if (decision[currentW, currentR, currentA, currentE, currentT, i] && !usedItemIds.Contains(item.GlobalId))
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

        var selectedTypes = selectedItems.Select(d => d.Original.PlaceType).ToHashSet();
        if (!requiredTypes.All(t => selectedTypes.Contains(t)))
        {
            Console.WriteLine($"Warning: Solution does not meet all priority requirements. Missing types: {string.Join(", ", requiredTypes.Except(selectedTypes))}");
        }

        Console.WriteLine($"Backtracking Complete: Selected {selectedItems.Count} Items");
        return selectedItems;
    }
}