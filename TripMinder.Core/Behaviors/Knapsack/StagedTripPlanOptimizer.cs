namespace TripMinder.Core.Behaviors.Knapsack;

using System.Collections.Generic;
using System.Threading.Tasks;

public class StagedTripPlanOptimizer : IStagedTripPlanOptimizer
    {
        private readonly IKnapsackSolver _solver;

        public StagedTripPlanOptimizer(IKnapsackSolver solver)
        {
            _solver = solver;
        }

        public async Task<List<Item>> OptimizeStagedAsync(
            List<Item> items,
            List<string> orderedInterests,
            int budget,
            UserDefinedKnapsackConstraints originalConstraints,
            (int a, int f, int e, int t)? priorities = null)
        {
            var selectedItems = new List<Item>();
            var currentCounts = new Dictionary<ItemType, int>
            {
                { ItemType.Restaurant, 0 },
                { ItemType.Accommodation, 0 },
                { ItemType.Entertainment, 0 },
                { ItemType.TourismArea, 0 }
            };
            var currentConstraints = new UserDefinedKnapsackConstraints(
                originalConstraints.MaxRestaurants,
                originalConstraints.MaxAccommodations,
                originalConstraints.MaxEntertainments,
                originalConstraints.MaxTourismAreas);
            int stablePhases = 0;
            int stage = 1;

            while (stablePhases < 4 && budget > 0)
            {
                bool added = false;
                foreach (var interest in orderedInterests)
                {
                    var itemType = GetItemType(interest);
                    if (currentCounts[itemType] >= currentConstraints.GetMaxCount(itemType))
                    {
                        Console.WriteLine($"Stage {stage}: Skipping {itemType}, max count reached: {currentCounts[itemType]}/{currentConstraints.GetMaxCount(itemType)}");
                        continue;
                    }

                    var availableItems = items.Except(selectedItems).Where(i => i.PlaceType == itemType).ToList();
                    if (!availableItems.Any())
                    {
                        Console.WriteLine($"Stage {stage}: No available items for {itemType}");
                        currentConstraints = UpdateConstraints(currentConstraints, itemType, currentCounts[itemType]);
                        continue;
                    }

                    var stageConstraints = new UserDefinedKnapsackConstraints(
                        itemType == ItemType.Restaurant ? currentCounts[ItemType.Restaurant] + 1 : currentCounts[ItemType.Restaurant],
                        itemType == ItemType.Accommodation ? currentCounts[ItemType.Accommodation] + 1 : currentCounts[ItemType.Accommodation],
                        itemType == ItemType.Entertainment ? currentCounts[ItemType.Entertainment] + 1 : currentCounts[ItemType.Entertainment],
                        itemType == ItemType.TourismArea ? currentCounts[ItemType.TourismArea] + 1 : currentCounts[ItemType.TourismArea]
                    );

                    var (maxProfit, stageItems) = await Task.Run(() => _solver.GetMaxProfit(
                        budget,
                        availableItems,
                        stageConstraints,
                        priorities,
                        true)); // requireExact = true

                    var newItem = stageItems.FirstOrDefault(i => i.PlaceType == itemType);
                    if (newItem != null && budget >= (int)newItem.AveragePricePerAdult)
                    {
                        selectedItems.Add(newItem);
                        budget -= (int)newItem.AveragePricePerAdult;
                        currentCounts[itemType]++;
                        added = true;
                        stablePhases = 0; // Reset stable phases
                        Console.WriteLine($"Stage {stage}: Added {newItem.Name} (Type={newItem.PlaceType}, Price={newItem.AveragePricePerAdult}, Score={newItem.Score}), Budget={budget}, Count={currentCounts[itemType]}");
                    }
                    else
                    {
                        Console.WriteLine($"Stage {stage}: Failed to add item for {itemType}. Selected={stageItems.Count}, BudgetCheck={(newItem == null ? "No item" : (budget >= (int)newItem.AveragePricePerAdult).ToString())}");
                        currentConstraints = UpdateConstraints(currentConstraints, itemType, currentCounts[itemType]);
                    }
                }

                if (!added)
                {
                    stablePhases++;
                    Console.WriteLine($"Stage {stage}: No items added, stable phases={stablePhases}");
                }
                stage++;
            }

            Console.WriteLine($"Optimization Complete: Total Items={selectedItems.Count}, Final Budget={budget}, Stable Phases={stablePhases}");
            return selectedItems;
        }

        private ItemType GetItemType(string interest)
        {
            return interest?.Trim().ToLowerInvariant() switch
            {
                "accommodation" => ItemType.Accommodation,
                "restaurants" or "food" => ItemType.Restaurant,
                "entertainments" or "entertainment" => ItemType.Entertainment,
                "tourismareas" or "tourism" => ItemType.TourismArea,
                _ => throw new ArgumentException($"Unknown interest: {interest}")
            };
        }

        private UserDefinedKnapsackConstraints UpdateConstraints(UserDefinedKnapsackConstraints constraints, ItemType itemType, int currentCount)
        {
            return new UserDefinedKnapsackConstraints(
                itemType == ItemType.Restaurant ? currentCount : constraints.MaxRestaurants,
                itemType == ItemType.Accommodation ? currentCount : constraints.MaxAccommodations,
                itemType == ItemType.Entertainment ? currentCount : constraints.MaxEntertainments,
                itemType == ItemType.TourismArea ? currentCount : constraints.MaxTourismAreas
            );
        }
    }

    public static class KnapsackConstraintsExtensions
    {
        public static int GetMaxCount(this IKnapsackConstraints constraints, ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Restaurant => constraints.MaxRestaurants,
                ItemType.Accommodation => constraints.MaxAccommodations,
                ItemType.Entertainment => constraints.MaxEntertainments,
                ItemType.TourismArea => constraints.MaxTourismAreas,
                _ => throw new ArgumentException($"Unknown item type: {itemType}")
            };
        }
    }
    