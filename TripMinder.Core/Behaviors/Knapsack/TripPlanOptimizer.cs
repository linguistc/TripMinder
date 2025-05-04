using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Behaviors.Shared;
using TripMinder.Core.Bases;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Behaviors.Knapsack;

public partial class TripPlanOptimizer
{
    private readonly IKnapsackSolver _solver;
    private readonly IItemFetcher _itemFetcher;
    private readonly IStagedTripPlanOptimizer _stagedOptimizer;
    private readonly IAccomodationService _accomodationService;
    private readonly IRestaurantService _restaurantService;
    private readonly IEntertainmentService _entertainmentService;
    private readonly ITourismAreaService _tourismAreaService;

    public TripPlanOptimizer(
        IKnapsackSolver solver,
        IItemFetcher itemFetcher,
        IStagedTripPlanOptimizer stagedOptimizer,
        IAccomodationService accomodationService,
        IRestaurantService restaurantService,
        IEntertainmentService entertainmentService,
        ITourismAreaService tourismAreaService)
    {
        _solver = solver;
        _itemFetcher = itemFetcher;
        _stagedOptimizer = stagedOptimizer;
        _accomodationService = accomodationService;
        _restaurantService = restaurantService;
        _entertainmentService = entertainmentService;
        _tourismAreaService = tourismAreaService;
    }

    public async Task<Respond<TripPlanResponse>> OptimizePlanPhasedAsync(TripPlanRequest request)
    {
        // 1. Check if optimization is feasible
        // if (!await CanRunOptimizationAsync(request.BudgetPerAdult))
        // {
        //     return new Respond<TripPlanResponse>
        //     {
        //         Succeeded = false,
        //         Message = "Budget too low to generate a trip plan",
        //         Errors = new List<string> { "Budget is less than the minimum item price" }
        //     };
        // }

        // 2. Calculate priorities and fetch items
        var priorities = CalculatePriorities(new Queue<string>(request.Interests));
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities, request.BudgetPerAdult);

        // 3. Determine interested types and filter items
        var interestTypes = new List<ItemType>();
        if (priorities.accommodation > 0) interestTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) interestTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) interestTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) interestTypes.Add(ItemType.TourismArea);

        var filteredItems = allItems.Where(i => interestTypes.Contains(i.PlaceType)).ToList();
        if (!filteredItems.Any())
        {
            return new Respond<TripPlanResponse>
            {
                Succeeded = false,
                Message = "No items available for the selected interests",
                Errors = new List<string> { "No valid items found" }
            };
        }

        // 4. Link type to priority weight
        var typeToPriorityWeight = new Dictionary<ItemType, double>
        {
            [ItemType.Accommodation] = 1.0 + priorities.accommodation * 0.5,
            [ItemType.Restaurant] = 1.0 + priorities.food * 0.5,
            [ItemType.Entertainment] = 1.0 + priorities.entertainment * 0.5,
            [ItemType.TourismArea] = 1.0 + priorities.tourism * 0.5
        };

        // 5. Recalculate scores with priority weighting
        foreach (var item in filteredItems)
        {
            item.Score = CalculateScoreBehavior.CalculateScore(
                item.ClassType,
                priorities.accommodation, // Use highest priority for consistency
                item.AveragePricePerAdult,
                request.BudgetPerAdult
            ) * (float)typeToPriorityWeight[item.PlaceType];
        }

        // 6. Prepare constraints
        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas
        );

        // 7. Run phased optimization with original interests order
        var bestItems = await _stagedOptimizer.OptimizeStagedAsync(
            filteredItems,
            request.Interests.ToList(), // Convert Queue<string> to List<string>
            (int)request.BudgetPerAdult,
            constraints,
            priorities
        );

        // 8. Build response
        var tripPlanResponse = BuildTripPlanResponse(bestItems, request);
        if (bestItems.Any())
        {
            return new Respond<TripPlanResponse>
            {
                Succeeded = true,
                Message = "Trip plan optimized successfully (Phased Expansion)",
                Data = tripPlanResponse,
                Meta = new { TotalItems = bestItems.Count }
            };
        }
        return new Respond<TripPlanResponse>
        {
            Succeeded = false,
            Message = "No valid trip plan found (Phased Expansion)",
            Errors = new List<string> { "Unable to generate a solution within constraints" }
        };
    }

    private async Task<bool> CanRunOptimizationAsync(double budget)
    {
        var minPrices = new[]
        {
            await _accomodationService.GetMinimumPriceAsync(),
            await _restaurantService.GetMinimumPriceAsync(),
            await _entertainmentService.GetMinimumPriceAsync(),
            await _tourismAreaService.GetMinimumPriceAsync()
        };

        var minPrice = minPrices.Where(p => p.HasValue).Min();
        return minPrice.HasValue && budget >= minPrice.Value;
    }

    private ItemType GetItemType(string interest)
    {
        return interest?.Trim().ToLowerInvariant() switch
        {
            "accommodation" => ItemType.Accommodation,
            "restaurants" or "food" => ItemType.Restaurant,
            "entertainments" or "entertainment" => ItemType.Entertainment,
            "tourismareas" or "tourism" => ItemType.TourismArea,
            _ => ItemType.Restaurant // Default
        };
    }

    public TripPlanResponse BuildTripPlanResponse(List<Item> selectedItems, TripPlanRequest request)
    {
        var response = new TripPlanResponse
        {
            Accommodation = selectedItems.FirstOrDefault(i => i.PlaceType == ItemType.Accommodation)?.ToResponse(),
            Restaurants = selectedItems.Where(i => i.PlaceType == ItemType.Restaurant)
                .Take(request.MaxRestaurants).Select(i => i.ToResponse()).ToList(),
            Entertainments = selectedItems.Where(i => i.PlaceType == ItemType.Entertainment)
                .Take(request.MaxEntertainments).Select(i => i.ToResponse()).ToList(),
            TourismAreas = selectedItems.Where(i => i.PlaceType == ItemType.TourismArea)
                .Take(request.MaxTourismAreas).Select(i => i.ToResponse()).ToList()
        };

        Console.WriteLine($"Built Response: Accommodation={(response.Accommodation?.Name ?? "None")}, Restaurants={response.Restaurants.Count}, Entertainments={response.Entertainments.Count}, TourismAreas={response.TourismAreas.Count}");
        return response;
    }

    public (int accommodation, int food, int entertainment, int tourism) CalculatePriorities(Queue<string> interests)
    {
        int accommodationPriority = 0, foodPriority = 0, entertainmentPriority = 0, tourismPriority = 0;
        int bonus = interests?.Count ?? 0;

        Console.WriteLine($"Calculating priorities for interests: {string.Join(", ", interests ?? new Queue<string>())}");

        if (interests != null)
        {
            int index = 0;
            foreach (var interest in interests)
            {
                var normalizedInterest = interest?.Trim().ToLowerInvariant();
                Console.WriteLine($"Processing interest: {normalizedInterest}");
                switch (normalizedInterest)
                {
                    case "accommodation":
                        accommodationPriority = bonus - index;
                        break;
                    case "restaurants":
                    case "food":
                        foodPriority = bonus - index;
                        break;
                    case "entertainments":
                    case "entertainment":
                        entertainmentPriority = bonus - index;
                        break;
                    case "tourismareas":
                    case "tourism":
                        tourismPriority = bonus - index;
                        break;
                    default:
                        Console.WriteLine($"Unrecognized interest: {normalizedInterest}");
                        break;
                }
                index++;
            }
        }

        if ((interests?.Any() ?? false) && accommodationPriority == 0 && foodPriority == 0 && entertainmentPriority == 0 && tourismPriority == 0)
        {
            Console.WriteLine("No valid priorities set, defaulting to all priorities");
            accommodationPriority = foodPriority = entertainmentPriority = tourismPriority = 1;
        }

        Console.WriteLine($"Calculated Priorities: Accommodation={accommodationPriority}, Food={foodPriority}, Entertainment={entertainmentPriority}, Tourism={tourismPriority}");
        return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
    }
    
    public async Task<Respond<List<TripPlanResponse>>> OptimizePlanMultiple(TripPlanRequest request)
    {
        var priorities = CalculatePriorities(request.Interests);
        Console.WriteLine(
            $"Calculated Priorities: Accommodation={priorities.accommodation}, Food={priorities.food}, Entertainment={priorities.entertainment}, Tourism={priorities.tourism}");

        var allItems =
            await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities, request.BudgetPerAdult);
        Console.WriteLine(
            $"Fetched Items: Total={allItems.Count}, Restaurants={allItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={allItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={allItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={allItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", allItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        var desiredTypes = new HashSet<ItemType>();
        if (priorities.accommodation > 0) desiredTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
        var filteredItems = allItems.Where(i => desiredTypes.Contains(i.PlaceType)).ToList();
        Console.WriteLine(
            $"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", filteredItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        var totalBudget = (int)(request.BudgetPerAdult);

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        var (maxProfit, allSolutions) =
            _solver.GetMaxProfitMultiple(totalBudget, filteredItems, constraints, priorities);
        var tripPlans = allSolutions.Select(items => BuildTripPlanResponse(items, request)).ToList();

        if (tripPlans.Any())
        {
            Console.WriteLine(
                $"Generated {tripPlans.Count} trip plans with total items: {tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0))}");
            return new Respond<List<TripPlanResponse>>
            {
                Succeeded = true,
                Message = "Trip plans optimized successfully",
                Data = tripPlans,
                Meta = new
                {
                    TotalItems = tripPlans.Sum(p =>
                        p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count +
                        (p.Accommodation != null ? 1 : 0)),
                    TotalSolutions = allSolutions.Count
                }
            };
        }

        Console.WriteLine("No valid trip plans generated.");
        return new Respond<List<TripPlanResponse>>
        {
            Succeeded = false,
            Message = "No valid trip plans found",
            Errors = new List<string> { "Unable to generate solutions within constraints" }
        };
    }
}