using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors.Shared;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Behaviors.Knapsack;

public class GreedyTripPlanner
{
    private readonly IGreedyTripSolver _solver;
    private readonly IItemFetcher _itemFetcher;
    private readonly IGreedyStagedTripPlanOptimizer _phaseOptimizer;
    private readonly IAccomodationService _accomodationService;
    private readonly IRestaurantService _restaurantService;
    private readonly IEntertainmentService _entertainmentService;
    private readonly ITourismAreaService _tourismAreaService;

    public GreedyTripPlanner(
        IGreedyTripSolver solver,
        IItemFetcher itemFetcher,
        IGreedyStagedTripPlanOptimizer phaseOptimizer,
        IAccomodationService accomodationService,
        IRestaurantService restaurantService,
        IEntertainmentService entertainmentService,
        ITourismAreaService tourismAreaService)
    {
        _solver = solver;
        _itemFetcher = itemFetcher;
        _phaseOptimizer = phaseOptimizer;
        _accomodationService = accomodationService;
        _restaurantService = restaurantService;
        _entertainmentService = entertainmentService;
        _tourismAreaService = tourismAreaService;
    }

    public async Task<Respond<TripPlanResponse>> PlanTripAsync(TripPlanRequest request)
    {
        // 1. Calculate priorities and fetch items
        var priorities = CalculatePriorities(request.Interests);
        Console.WriteLine($"Priorities: A={priorities.accommodation}, F={priorities.food}, E={priorities.entertainment}, T={priorities.tourism}");
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities, request.BudgetPerAdult);

        // 2. Include all interest types from orderedInterests, even if priority is zero
        var interestTypes = request.Interests.Select(GetItemType).Distinct().ToList();
        var filteredItems = allItems.Where(i => interestTypes.Contains(i.PlaceType)).ToList();
        if (!filteredItems.Any())
        {
            Console.WriteLine("No items available for selected interests.");
            return new Respond<TripPlanResponse>
            {
                Succeeded = false,
                Message = "No items available for the selected interests",
                Errors = new List<string> { "No valid items found" }
            };
        }

        // 3. Apply priority weights and recalculate scores
        var typeToPriorityWeight = new Dictionary<ItemType, double>
        {
            [ItemType.Accommodation] = 1.0 + priorities.accommodation * 0.5,
            [ItemType.Restaurant] = 1.0 + priorities.food * 0.5,
            [ItemType.Entertainment] = 1.0 + priorities.entertainment * 0.5,
            [ItemType.TourismArea] = 1.0 + priorities.tourism * 0.5
        };

        foreach (var item in filteredItems)
        {
            int p = item.PlaceType switch
            {
                ItemType.Accommodation => priorities.accommodation,
                ItemType.Restaurant => priorities.food,
                ItemType.Entertainment => priorities.entertainment,
                ItemType.TourismArea => priorities.tourism,
                _ => 1
            };
            var baseScore = CalculateScoreBehavior.CalculateScore(
                item.ClassType,
                p,
                item.AveragePricePerAdult,
                request.BudgetPerAdult,
                item.Rating
            );
            item.Score = baseScore * (float)typeToPriorityWeight[item.PlaceType];
            Console.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Priority: {p}, BaseScore: {baseScore}, FinalScore: {item.Score}");
        }

        // 4. Prepare constraints
        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        // 5. Run phased optimization
        var bestItems = await _phaseOptimizer.OptimizeStagedAsync(
            filteredItems,
            request.Interests.ToList(),
            (int)request.BudgetPerAdult,
            constraints,
            priorities);

        // 6. Build response
        var response = BuildTripPlanResponse(bestItems, request);
        if (bestItems.Any())
        {
            Console.WriteLine($"Trip plan generated: Items={bestItems.Count}, Total Cost={bestItems.Sum(i => i.AveragePricePerAdult)}");
            return new Respond<TripPlanResponse>
            {
                Succeeded = true,
                Message = "Trip plan generated successfully (Greedy Phased)",
                Data = response,
                Meta = new { TotalItems = bestItems.Count }
            };
        }

        Console.WriteLine("No valid trip plan found.");
        return new Respond<TripPlanResponse>
        {
            Succeeded = false,
            Message = "No valid trip plan found (Greedy Phased)",
            Errors = new List<string> { "Unable to generate a plan within constraints" }
        };
    }

    private TripPlanResponse BuildTripPlanResponse(List<Item> selectedItems, TripPlanRequest request)
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

    private (int accommodation, int food, int entertainment, int tourism) CalculatePriorities(Queue<string> interests)
    {
        int accommodationPriority = 0, foodPriority = 0, entertainmentPriority = 0, tourismPriority = 0;
        int bonus = interests?.Count ?? 0;

        Console.WriteLine($"Calculating priorities: {string.Join(", ", interests ?? new Queue<string>())}");

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

        // Ensure all interest types have at least a minimal priority
        if (accommodationPriority == 0 && interests.Any(i => i.Trim().ToLowerInvariant() == "accommodation"))
            accommodationPriority = 1;
        if (foodPriority == 0 && interests.Any(i => i.Trim().ToLowerInvariant() == "food" || i.Trim().ToLowerInvariant() == "restaurants"))
            foodPriority = 1;
        if (entertainmentPriority == 0 && interests.Any(i => i.Trim().ToLowerInvariant() == "entertainment" || i.Trim().ToLowerInvariant() == "entertainments"))
            entertainmentPriority = 1;
        if (tourismPriority == 0 && interests.Any(i => i.Trim().ToLowerInvariant() == "tourism" || i.Trim().ToLowerInvariant() == "tourismareas"))
            tourismPriority = 1;

        Console.WriteLine($"Priorities: A={accommodationPriority}, F={foodPriority}, E={entertainmentPriority}, T={tourismPriority}");
        return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
    }

    private ItemType GetItemType(string interest) => interest?.Trim().ToLowerInvariant() switch
    {
        "accommodation" => ItemType.Accommodation,
        "restaurants" or "food" => ItemType.Restaurant,
        "entertainments" or "entertainment" => ItemType.Entertainment,
        "tourismareas" or "tourism" => ItemType.TourismArea,
        _ => ItemType.Restaurant
    };
}