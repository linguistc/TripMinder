using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using TripMinder.Core.Behaviors.Shared;

   namespace TripMinder.Core.Behaviors.Knapsack;

   public class GreedyTripSolver : IGreedyTripSolver
   {
       private readonly GreedySolutionOptimizer _optimizer;
       private readonly IGreedyStagedTripPlanOptimizer _phaseOptimizer;

       public GreedyTripSolver(GreedySolutionOptimizer optimizer, IGreedyStagedTripPlanOptimizer phaseOptimizer)
       {
           _optimizer = optimizer;
           _phaseOptimizer = phaseOptimizer;
       }

       public (float maxProfit, List<Item> selectedItems) GetBestPlan(
           int budget,
           List<Item> items,
           IKnapsackConstraints constraints,
           (int a, int f, int e, int t)? priorities = null)
       {
           var selectedItems = RunGreedy(budget, items, constraints, priorities).Result;
           float maxProfit = selectedItems.Sum(i => i.Score);
           Console.WriteLine($"Greedy Single Plan: Profit={maxProfit}, Items={selectedItems.Count}, Cost={selectedItems.Sum(i => i.AveragePricePerAdult)}");
           return (maxProfit, selectedItems);
       }

       public (float maxProfit, List<List<Item>> allPlans) GetMultiplePlans(
           int budget,
           List<Item> items,
           IKnapsackConstraints constraints,
           (int a, int f, int e, int t)? priorities = null)
       {
           var basePlan = RunGreedy(budget, items, constraints, priorities).Result;
           _optimizer.TryAddSolution(basePlan);

           var allPlans = _optimizer.GetTopSolutions();
           float maxProfit = allPlans.Any() ? allPlans.Max(s => s.Sum(i => i.Score)) : 0f;
           Console.WriteLine($"Greedy Multiple Plans: Count={allPlans.Count}, Max Profit={maxProfit}");
           return (maxProfit, allPlans);
       }

       private async Task<List<Item>> RunGreedy(
           int budget,
           List<Item> items,
           IKnapsackConstraints constraints,
           (int a, int f, int e, int t)? priorities)
       {
           var userConstraints = new UserDefinedKnapsackConstraints(
               constraints.MaxRestaurants,
               constraints.MaxAccommodations,
               constraints.MaxEntertainments,
               constraints.MaxTourismAreas);

           var orderedInterests = priorities.HasValue
               ? new List<(ItemType, int)>
                 {
                     (ItemType.Accommodation, priorities.Value.a),
                     (ItemType.Restaurant, priorities.Value.f),
                     (ItemType.Entertainment, priorities.Value.e),
                     (ItemType.TourismArea, priorities.Value.t)
                 }
                 .OrderByDescending(x => x.Item2)
                 .Select(x => x.Item1.ToString().ToLower())
                 .ToList()
               : new List<string> { "accommodation", "restaurants", "entertainments", "tourismareas" };

           return await _phaseOptimizer.OptimizeStagedAsync(items, orderedInterests, budget, userConstraints, priorities);
       }
   }