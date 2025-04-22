using TripMinder.Core.Bases;
using System.Collections.Generic;
using System.Linq;

namespace TripMinder.Core.Behaviors.Knapsack;

public partial class TripPlanOptimizer
{
    private readonly IKnapsackSolver _solver;
    private readonly IItemFetcher _itemFetcher;

    public TripPlanOptimizer(IKnapsackSolver solver, IItemFetcher itemFetcher)
    {
        _solver = solver;
        _itemFetcher = itemFetcher;
    }

     public async Task<Respond<TripPlanResponse>> OptimizePlan(TripPlanRequest request)
    {
        // 1. Calculate priorities
        var priorities = CalculatePriorities(request.Interests);
        Console.WriteLine($"Calculated Priorities: Accommodation={priorities.accommodation}, Food={priorities.food}, Entertainment={priorities.entertainment}, Tourism={priorities.tourism}");

        // 2. Fetch and filter items
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);
        var desiredTypes = new HashSet<ItemType>();
        if (priorities.accommodation > 0) desiredTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
        var filteredItems = allItems.Where(i => desiredTypes.Contains(i.PlaceType)).ToList();
        Console.WriteLine($"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}");

        // 3. Select baseline items
        int budget = (int)request.BudgetPerAdult; // Use full budget
        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);
        var baselineItems = PickBaselineItems(filteredItems, priorities, ref budget, ref constraints);
        Console.WriteLine($"Baseline Items Selected: {baselineItems.Count}, Items={string.Join(", ", baselineItems.Select(i => $"{i.Name} (Type={i.PlaceType}, Price={i.AveragePricePerAdult})"))}, Remaining Budget={budget}");

        // 4. Run DP on all items with full budget
        var (maxProfit, dpItems) = _solver.GetMaxProfit((int)request.BudgetPerAdult, allItems, constraints, priorities);
        Console.WriteLine($"DP Items Selected: {dpItems.Count}, Items={string.Join(", ", dpItems.Select(i => $"{i.Name} (Type={i.PlaceType}, Price={i.AveragePricePerAdult})"))}, Total Profit={maxProfit}");

        // 5. Combine baseline and DP items
        var finalItems = baselineItems.Concat(dpItems).DistinctBy(i => i.GlobalId).ToList();
        Console.WriteLine($"Final Items: {finalItems.Count}, Items={string.Join(", ", finalItems.Select(i => $"{i.Name} (Type={i.PlaceType}, Price={i.AveragePricePerAdult})"))}");
        var tripPlanResponse = BuildTripPlanResponse(finalItems, request);

        if (finalItems.Any())
        {
            return new Respond<TripPlanResponse>
            {
                Succeeded = true,
                Message = "Trip plan optimized successfully",
                Data = tripPlanResponse,
                Meta = new { TotalItems = finalItems.Count, TotalSolutions = 1 }
            };
        }

        return new Respond<TripPlanResponse>
        {
            Succeeded = false,
            Message = "No valid trip plan found",
            Errors = new List<string> { "Unable to generate a solution within constraints" }
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
                        accommodationPriority += bonus--;
                        break;
                    case "restaurants":
                    case "food":
                        foodPriority += bonus--;
                        break;
                    case "entertainments":
                    case "entertainment":
                        entertainmentPriority += bonus--;
                        break;
                    case "tourismareas":
                    case "tourism":
                        tourismPriority += bonus--;
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