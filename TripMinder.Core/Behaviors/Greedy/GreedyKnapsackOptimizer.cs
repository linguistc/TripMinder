using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Service.Contracts;
using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Greedy;

/// <summary>
/// Greedy-based trip plan optimizer: orders items by score and selects respecting budget and per-type limits.
/// </summary>
// public class GreedyKnapsackOptimizer
// {
//     private readonly IItemFetcher _itemFetcher;
//     private readonly IAccomodationService _accomodationService;
//     private readonly IRestaurantService _restaurantService;
//     private readonly IEntertainmentService _entertainmentService;
//     private readonly ITourismAreaService _tourismAreaService;
//
//     public GreedyKnapsackOptimizer(
//         ItemFetcher itemFetcher,
//         IAccomodationService accomodationService,
//         IRestaurantService restaurantService,
//         IEntertainmentService entertainmentService,
//         ITourismAreaService tourismAreaService)
//     {
//         _itemFetcher = itemFetcher;
//         _accomodationService = accomodationService;
//         _restaurantService = restaurantService;
//         _entertainmentService = entertainmentService;
//         _tourismAreaService = tourismAreaService;
//     }
//     
//     public async Task<Respond<TripPlanResponse>> GreedyOptimizeAsync(
//         TripPlanRequest request)
//     {
//         // 1. Feasibility check
//             // if (!await CanRunOptimizationAsync(request.BudgetPerAdult))
//             //     return BudgetTooLowResponse();
//
//             // 2. Fetch and filter items by user interests
//             var priorities = PriorityCalculator.Calculate(request.Interests);
//             var allItems = await _itemFetcher.FetchItems(
//                 request.GovernorateId,
//                 request.ZoneId,
//                 priorities,
//                 request.BudgetPerAdult);
//
//             var desiredTypes = new HashSet<ItemType>();
//             if (priorities.accomodation > 0) desiredTypes.Add(ItemType.Accommodation);
//             if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
//             if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
//             if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
//
//             var candidates = allItems
//                 .Where(i => desiredTypes.Contains(i.PlaceType))
//                 .ToList();
//
//             // 3. Compute score for each item: combine priority factor and profit/cost ratio
//             var scored = candidates.Select(i =>
//             {
//                 var ratio = i.Score / i.AveragePricePerAdult;
//                 var priorityWeight = priorities.GetPriority(i.PlaceType);
//                 i.Score = (float)priorityWeight + (float)ratio; // simple linear combination
//                 return i;
//             })
//             .OrderByDescending(i => i.Score)
//             .ToList();
//
//             // 4. Greedy selection respecting budget and per-type limits
//             var result = new List<Item>();
//             double remainingBudget = request.BudgetPerAdult;
//             var counts = new Dictionary<ItemType, int>
//             {
//                 [ItemType.Accommodation] = 0,
//                 [ItemType.Restaurant] = 0,
//                 [ItemType.Entertainment] = 0,
//                 [ItemType.TourismArea] = 0
//             };
//
//             foreach (var item in scored)
//             {
//                 var maxAllowed = request.GetMaxCount(item.PlaceType);
//                 if (counts[item.PlaceType] >= maxAllowed)
//                     continue;
//
//                 if (item.AveragePricePerAdult <= remainingBudget)
//                 {
//                     result.Add(item);
//                     counts[item.PlaceType]++;
//                     remainingBudget -= item.AveragePricePerAdult;
//                 }
//             }
//
//             // 5. Optional second pass to fill budget with any leftover cheap item
//             foreach (var item in scored)
//             {
//                 if (remainingBudget < scored.Min(x => x.AveragePricePerAdult))
//                     break;
//
//                 if (counts[item.PlaceType] < request.GetMaxCount(item.PlaceType)
//                     && item.AveragePricePerAdult <= remainingBudget)
//                 {
//                     result.Add(item);
//                     counts[item.PlaceType]++;
//                     remainingBudget -= item.AveragePricePerAdult;
//                 }
//             }
//
//             // 6. Build the response
//             var response = BuildTripPlanResponse(result, request);
//             return new Respond<TripPlanResponse>
//             {
//                 Succeeded = result.Any(),
//                 Message = result.Any() ? "Trip plan generated (Greedy)" : "No valid plan found",
//                 Data = response
//             };
//         }
//
//         private async Task<bool> CanRunOptimizationAsync(double budget)
//         {
//             var mins = new[]
//             {
//                 await _accomodationService.GetMinimumPriceAsync(),
//                 await _restaurantService.GetMinimumPriceAsync(),
//                 await _entertainmentService.GetMinimumPriceAsync(),
//                 await _tourismAreaService.GetMinimumPriceAsync()
//             };
//             var min = mins.Where(x => x.HasValue).Min();
//             return min.HasValue && budget >= min.Value;
//         }
//
//         private Respond<TripPlanResponse> BudgetTooLowResponse()
//             => new Respond<TripPlanResponse>
//             {
//                 Succeeded = false,
//                 Message = "Budget too low to generate a trip plan",
//                 Errors = new List<string> { "Budget below minimum item price" }
//             };
//     }

    // Extension methods and helpers
    public static class PriorityExtensions
    {
        public static int GetPriority(this (int accommodation, int food, int entertainment, int tourism) p, ItemType type)
        {
            return type switch
            {
                ItemType.Accommodation => p.accommodation,
                ItemType.Restaurant => p.food,
                ItemType.Entertainment => p.entertainment,
                ItemType.TourismArea => p.tourism,
                _ => 0
            };
        }

        public static int GetMaxCount(this TripPlanRequest req, ItemType type)
        {
            return type switch
            {
                ItemType.Accommodation => req.MaxAccommodations,
                ItemType.Restaurant => req.MaxRestaurants,
                ItemType.Entertainment => req.MaxEntertainments,
                ItemType.TourismArea => req.MaxTourismAreas,
                _ => 0
            };
        }
    }