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
        // 1. Calculate priorities and fetch items
        var priorities = CalculatePriorities(request.Interests);
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);

        // 2. Determine interested types and filter items
        var interestTypes = new List<ItemType>();
        if (priorities.accommodation > 0) interestTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) interestTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) interestTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) interestTypes.Add(ItemType.TourismArea);

        var filteredItems = allItems.Where(i => interestTypes.Contains(i.PlaceType)).ToList();

        // 3. Prepare max constraints per type
        var maxPerType = new Dictionary<ItemType, int>
        {
            [ItemType.Accommodation] = request.MaxAccommodations,
            [ItemType.Restaurant] = request.MaxRestaurants,
            [ItemType.Entertainment] = request.MaxEntertainments,
            [ItemType.TourismArea] = request.MaxTourismAreas
        };

        // 4. Prepare phased expansion variables
        var phaseOrder = interestTypes.ToList(); // Ordered by user priority
        var currentMax = phaseOrder.ToDictionary(t => t, t => 0);
        var lastSuccessMax = phaseOrder.ToDictionary(t => t, t => 0);
        var budget = (int)request.BudgetPerAdult;
        var bestItems = new List<Item>();

        // === Early Stop Phased Expansion Loop ===
        while (true)
        {
            bool changedInThisLoop = false;

            foreach (var type in phaseOrder)
            {
                // Don't exceed user max
                if (currentMax[type] >= maxPerType[type])
                    continue;

                // Prepare constraints for this phase
                var phaseConstraints = new UserDefinedKnapsackConstraints(
                    currentMax.GetValueOrDefault(ItemType.Restaurant) + (type == ItemType.Restaurant ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.Accommodation) + (type == ItemType.Accommodation ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.Entertainment) + (type == ItemType.Entertainment ? 1 : 0),
                    currentMax.GetValueOrDefault(ItemType.TourismArea) + (type == ItemType.TourismArea ? 1 : 0)
                );

                // Run knapsack for this phase (always with full budget)
                var (profit, items) = _solver.GetMaxProfit(
                    budget,
                    filteredItems,
                    phaseConstraints,
                    priorities
                );

                int countOfType = items.Count(i => i.PlaceType == type);
                if (countOfType > currentMax[type])
                {
                    // Success: update max and bestItems
                    currentMax[type]++;
                    lastSuccessMax[type] = currentMax[type];
                    bestItems = items;
                    changedInThisLoop = true;
                }
                else
                {
                    // Failed to add more: fix max at last successful
                    currentMax[type] = lastSuccessMax[type];
                }
            }

            // Early stop: if no type increased in this full loop, or all max reached
            if (!changedInThisLoop || phaseOrder.All(t => currentMax[t] >= maxPerType[t]))
                break;

            // Also, if budget is less than min price of any available item, stop
            var minPrices = phaseOrder
                .Select(t => filteredItems.Where(i => i.PlaceType == t).Select(i => i.AveragePricePerAdult).DefaultIfEmpty(double.MaxValue).Min())
                .ToList();
            if (budget < minPrices.Min())
                break;
        }

        // Build response
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

    private int GetMaxCount(ItemType itemType, IKnapsackConstraints constraints)
    {
        return itemType switch
        {
            ItemType.Restaurant => constraints.MaxRestaurants,
            ItemType.Accommodation => constraints.MaxAccommodations,
            ItemType.Entertainment => constraints.MaxEntertainments,
            ItemType.TourismArea => constraints.MaxTourismAreas,
            _ => 0
        };
    }

    private TripPlanResponse BuildTripPlanResponse(List<Item> selectedItems, TripPlanRequest request)
    {
        // Existing implementation
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
        // Existing implementation
        int accommodationPriority = 0, foodPriority = 0, entertainmentPriority = 0, tourismPriority = 0;
        int bonus = interests?.Count ?? 0;

        Console.WriteLine($"Calculating priorities for interests: {string.Join(", ", interests ?? new Queue<string>())}");

        if (interests != null)
        {
            foreach (var interest in interests)
            {
                var normalizedInterest = interest?.Trim().ToLowerInvariant();
                Console.WriteLine($"Processing interest: {normalizedInterest}");
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
                    default:
                        Console.WriteLine($"Unrecognized interest: {normalizedInterest}");
                        break;
                }
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

    
    // Un touched yet
    public async Task<Respond<List<TripPlanResponse>>> OptimizePlanMultiple(TripPlanRequest request)
    {
        var priorities = CalculatePriorities(request.Interests);
        Console.WriteLine($"Calculated Priorities: Accommodation={priorities.accommodation}, Food={priorities.food}, Entertainment={priorities.entertainment}, Tourism={priorities.tourism}");

        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);
        Console.WriteLine($"Fetched Items: Total={allItems.Count}, Restaurants={allItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={allItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={allItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={allItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", allItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        // Filter items based on interests
        var desiredTypes = new HashSet<ItemType>();
        if (priorities.accommodation > 0) desiredTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
        var filteredItems = allItems.Where(i => desiredTypes.Contains(i.PlaceType)).ToList();
        Console.WriteLine($"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", filteredItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        var totalBudget = (int)(request.BudgetPerAdult);

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        var (maxProfit, allSolutions) = _solver.GetMaxProfitMultiple(totalBudget, filteredItems, constraints, priorities);
        var tripPlans = allSolutions.Select(items => BuildTripPlanResponse(items, request)).ToList();

        if (tripPlans.Any())
        {
            Console.WriteLine($"Generated {tripPlans.Count} trip plans with total items: {tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0))}");
            return new Respond<List<TripPlanResponse>>
            {
                Succeeded = true,
                Message = "Trip plans optimized successfully",
                Data = tripPlans,
                Meta = new { TotalItems = tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0)), TotalSolutions = allSolutions.Count }
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


// HELPER CLASSES
public record TripPlanRequest(
    int GovernorateId,
    int? ZoneId, 
    double BudgetPerAdult, 
    int NumberOfTravelers, 
    Queue<string> Interests, 
    int MaxRestaurants, 
    int MaxAccommodations, 
    int MaxEntertainments, 
    int MaxTourismAreas);
public record TripPlanResponse
{
    public ItemResponse Accommodation { get; init; }
    public List<ItemResponse> Restaurants { get; init; }
    public List<ItemResponse> Entertainments { get; init; }
    public List<ItemResponse> TourismAreas { get; init; }
}

public record ItemResponse(int Id, string Name, string ClassType, double AveragePricePerAdult, float Score, ItemType PlaceType, double Rating, string ImageSource);

public enum ItemType { Accommodation, Restaurant, Entertainment, TourismArea }

public class Item
{
    public int Id { get; set; }
    public string GlobalId { get; set; }
    public string Name { get; set; }
    public string ClassType { get; set; }
    public double AveragePricePerAdult { get; set; }
    public float Score { get; set; }
    public ItemType PlaceType { get; set; }
    public double Rating { get; set; }
    public string ImageSource { get; set; }
    public ItemResponse ToResponse() => new ItemResponse(Id, Name, ClassType, AveragePricePerAdult, Score, PlaceType, Rating, ImageSource);
}