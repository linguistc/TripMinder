using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Behaviors.Knapsack;

public class TripPlanOptimizer
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
        var priorities = CalculatePriorities(request.Interests);
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId ,request.ZoneId, priorities);
        var totalBudget = (int)(request.BudgetPerAdult);

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        var (maxProfit, selectedItems) = _solver.GetMaxProfit(totalBudget, allItems, constraints, priorities);
        var tripPlanResponse = BuildTripPlanResponse(selectedItems, request);
        return new Respond<TripPlanResponse>
        {
            Succeeded = true,
            Message = "Trip plan optimized successfully",
            Data = tripPlanResponse,
            Meta = new { TotalItems = selectedItems.Count, TotalSolutions = 1 }
        };
    }

    public async Task<Respond<List<TripPlanResponse>>> OptimizePlanMultiple(TripPlanRequest request)
    {
        var priorities = CalculatePriorities(request.Interests);
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId ,request.ZoneId, priorities);
        var totalBudget = (int)(request.BudgetPerAdult);

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        var (maxProfit, allSolutions) = _solver.GetMaxProfitMultiple(totalBudget, allItems, constraints, priorities);
        var tripPlans = allSolutions.Select(items => BuildTripPlanResponse(items, request)).ToList();

        return new Respond<List<TripPlanResponse>>
        {
            Succeeded = true,
            Message = "Trip plans optimized successfully",
            Data = tripPlans,
            Meta = new { TotalItems = tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0)), TotalSolutions = allSolutions.Count }
        };
    }

    private TripPlanResponse BuildTripPlanResponse(List<Item> selectedItems, TripPlanRequest request)
    {
        return new TripPlanResponse
        {
            Accommodation = selectedItems.FirstOrDefault(i => i.PlaceType == ItemType.Accommodation)?.ToResponse(),
            Restaurants = selectedItems.Where(i => i.PlaceType == ItemType.Restaurant)
                .Take(request.MaxRestaurants).Select(i => i.ToResponse()).ToList(),
            Entertainments = selectedItems.Where(i => i.PlaceType == ItemType.Entertainment)
                .Take(request.MaxEntertainments).Select(i => i.ToResponse()).ToList(),
            TourismAreas = selectedItems.Where(i => i.PlaceType == ItemType.TourismArea)
                .Take(request.MaxTourismAreas).Select(i => i.ToResponse()).ToList()
        };
    }

    private (int accommodation, int food, int entertainment, int tourism) CalculatePriorities(Queue<string> interests)
    {
        int accommodationPriority = 1, foodPriority = 1, entertainmentPriority = 1, tourismPriority = 1;
        int bonus = interests.Count; // بدل ما نستخدم الأولوية كرقم كبير، نعطي مكافأة صغيرة
        while (interests.Count > 0)
        {
            var interest = interests.Dequeue();
            switch (interest.ToLower())
            {
                case "accommodation": accommodationPriority += bonus--; break;
                case "restaurants": case "food": foodPriority += bonus--; break;
                case "entertainments": case "entertainment": entertainmentPriority += bonus--; break;
                case "tourismareas": case "tourism": tourismPriority += bonus--; break;
            }
        }

        return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
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

public record ItemResponse(int Id, string Name, double AveragePricePerAdult, float Score, ItemType PlaceType);

public enum ItemType { Accommodation, Restaurant, Entertainment, TourismArea }

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double AveragePricePerAdult { get; set; }
    public float Score { get; set; }
    public ItemType PlaceType { get; set; }
    public ItemResponse ToResponse() => new ItemResponse(Id, Name, AveragePricePerAdult, Score, PlaceType);
}