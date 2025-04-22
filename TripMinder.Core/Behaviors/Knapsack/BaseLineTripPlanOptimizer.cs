using TripMinder.Core.Bases;
using System.Collections.Generic;
using System.Linq;

namespace TripMinder.Core.Behaviors.Knapsack;

public partial class TripPlanOptimizer
{
    private List<Item> PickBaselineItems(
        List<Item> items,
        (int a, int f, int e, int t) priorities,
        ref int budget,
        ref UserDefinedKnapsackConstraints constraints)
    {
        var baseline = new List<Item>();
        var map = new Dictionary<ItemType, int>
        {
            { ItemType.Accommodation, priorities.a },
            { ItemType.Restaurant, priorities.f },
            { ItemType.Entertainment, priorities.e },
            { ItemType.TourismArea, priorities.t }
        };

        Console.WriteLine($"Picking baseline items: Budget={budget}, Priorities={string.Join(", ", map.Select(kv => $"{kv.Key}: {kv.Value}"))}");

        foreach (var kv in map.Where(kv => kv.Value > 0))
        {
            var candidates = items
                .Where(i => i.PlaceType == kv.Key)
                .Select(i => new
                {
                    Item = i,
                    Score = CalculateScoreBehavior.CalculateScore(i.ClassType, kv.Value, i.AveragePricePerAdult)
                })
                .ToList();

            Console.WriteLine($"Candidates for {kv.Key}: Count={candidates.Count}, Items={string.Join(", ", candidates.Select(c => $"{c.Item.Name} (Score={c.Score}, Price={c.Item.AveragePricePerAdult})"))}");

            if (!candidates.Any())
            {
                Console.WriteLine($"No candidates found for {kv.Key}");
                continue;
            }

            // Select highest score, break ties by cheapest price
            var best = candidates
                .OrderByDescending(c => c.Score)
                .ThenBy(c => c.Item.AveragePricePerAdult)
                .First()
                .Item;

            if (budget >= (int)best.AveragePricePerAdult)
            {
                baseline.Add(best);
                budget -= (int)best.AveragePricePerAdult;
                Console.WriteLine($"Baseline selected: {best.Name}, Type={best.PlaceType}, Score={best.Score}, Price={best.AveragePricePerAdult}, Remaining Budget={budget}");

                // Update constraints
                switch (best.PlaceType)
                {
                    case ItemType.Accommodation:
                        constraints = new UserDefinedKnapsackConstraints(
                            constraints.MaxRestaurants,
                            constraints.MaxAccommodations - 1,
                            constraints.MaxEntertainments,
                            constraints.MaxTourismAreas);
                        break;
                    case ItemType.Restaurant:
                        constraints = new UserDefinedKnapsackConstraints(
                            constraints.MaxRestaurants - 1,
                            constraints.MaxAccommodations,
                            constraints.MaxEntertainments,
                            constraints.MaxTourismAreas);
                        break;
                    case ItemType.Entertainment:
                        constraints = new UserDefinedKnapsackConstraints(
                            constraints.MaxRestaurants,
                            constraints.MaxAccommodations,
                            constraints.MaxEntertainments - 1,
                            constraints.MaxTourismAreas);
                        break;
                    case ItemType.TourismArea:
                        constraints = new UserDefinedKnapsackConstraints(
                            constraints.MaxRestaurants,
                            constraints.MaxAccommodations,
                            constraints.MaxEntertainments,
                            constraints.MaxTourismAreas - 1);
                        break;
                }
            }
            else
            {
                Console.WriteLine($"Insufficient budget for {best.Name}, Type={best.PlaceType}, Price={best.AveragePricePerAdult}, Remaining Budget={budget}");
            }
        }

        // Remove baseline items from input list
        items.RemoveAll(i => baseline.Any(b => b.GlobalId == i.GlobalId));
        Console.WriteLine($"Baseline items selected: {baseline.Count}, Items={string.Join(", ", baseline.Select(i => i.Name))}");
        return baseline;
    }
}