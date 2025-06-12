using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors.Shared;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Behaviors.Knapsack;

public class GreedyTripPlanner
{
    private readonly IGreedyTripSolver _solver;
    private readonly IItemFetcher _itemFetcher;
    private readonly IGreedyPhaseOptimizer _phaseOptimizer;
    private readonly IAccomodationService _accomodationService;
    private readonly IRestaurantService _restaurantService;
    private readonly IEntertainmentService _entertainmentService;
    private readonly ITourismAreaService _tourismAreaService;

    public GreedyTripPlanner(
        IGreedyTripSolver solver,
        IItemFetcher itemFetcher,
        IGreedyPhaseOptimizer phaseOptimizer,
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
        var priorities = CalculatePriorities(request.Interests);
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities, request.BudgetPerAdult);

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

        var typeToPriority = new Dictionary<ItemType, int>
        {
            [ItemType.Accommodation] = priorities.accommodation,
            [ItemType.Restaurant] = priorities.food,
            [ItemType.Entertainment] = priorities.entertainment,
            [ItemType.TourismArea] = priorities.tourism
        };

        foreach (var item in filteredItems)
        {
            item.Score = CalculateScoreBehavior.CalculateScore(
                item.ClassType,
                typeToPriority[item.PlaceType],
                item.AveragePricePerAdult,
                request.BudgetPerAdult, 1
            );
        }

        var orderedInterests = request.Interests
            .Select((interest, index) => (interest, priority: typeToPriority[GetItemType(interest)]))
            .OrderByDescending(x => x.priority)
            .Select(x => x.interest)
            .ToList();

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        var bestItems = await _phaseOptimizer.OptimizePhasedAsync(
            filteredItems,
            orderedInterests,
            (int)request.BudgetPerAdult,
            constraints,
            priorities);

        var response = BuildTripPlanResponse(bestItems, request);
        if (bestItems.Any())
        {
            return new Respond<TripPlanResponse>
            {
                Succeeded = true,
                Message = "Trip plan generated successfully",
                Data = response,
                Meta = new { TotalItems = bestItems.Count }
            };
        }

        return new Respond<TripPlanResponse>
        {
            Succeeded = false,
            Message = "No valid trip plan found",
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

        if (interests != null)
        {
            foreach (var interest in interests)
            {
                var normalizedInterest = interest?.Trim().ToLowerInvariant();
                switch (normalizedInterest)
                {
                    case "accommodation":
                        accommodationPriority = bonus--;
                        break;
                    case "restaurants":
                    case "food":
                        foodPriority = bonus--;
                        break;
                    case "entertainments":
                    case "entertainment":
                        entertainmentPriority = bonus--;
                        break;
                    case "tourismareas":
                    case "tourism":
                        tourismPriority = bonus--;
                        break;
                }
            }
        }

        if ((interests?.Any() ?? false) && accommodationPriority == 0 && foodPriority == 0 && entertainmentPriority == 0 && tourismPriority == 0)
        {
            accommodationPriority = foodPriority = entertainmentPriority = tourismPriority = 1;
        }

        return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
    }

    private ItemType GetItemType(string interest)
    {
        return interest?.Trim().ToLowerInvariant() switch
        {
            "accommodation" => ItemType.Accommodation,
            "restaurants" or "food" => ItemType.Restaurant,
            "entertainments" or "entertainment" => ItemType.Entertainment,
            "tourismareas" or "tourism" => ItemType.TourismArea,
            _ => ItemType.Restaurant
        };
    }
}