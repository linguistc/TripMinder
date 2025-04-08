using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Behaviors.Knapsack;

public class TripPlanOptimizer
{
    private readonly IMediator _mediator;
    private readonly IKnapsackSolver _solver;
    private readonly IItemFetcher _itemFetcher;

    public TripPlanOptimizer(IMediator mediator, IKnapsackSolver solver, IItemFetcher itemFetcher)
    {
        this._mediator = mediator;
        this._solver = solver;
        this._itemFetcher = itemFetcher;
    }

    public async Task<Respond<TripPlanResponse>> OptimizePlan(TripPlanRequest request)
    {
        var priorities = CalculatePriorities(request.Interests);
        var allItems = await _itemFetcher.FetchItems(request.ZoneId, priorities, _mediator);
        var totalBudget = (int)(request.BudgetPerAdult * request.NumberOfTravelers);

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);
        
        var (maxProfit, selectedItems) = _solver.GetMaxProfit(totalBudget, allItems, constraints);

        var tripPlanResponse = new TripPlanResponse
        {
            Accommodation = selectedItems.FirstOrDefault(i => i.PlaceType == ItemType.Accommodation)?.ToResponse(),
            Restaurants = selectedItems.Where(i => i.PlaceType == ItemType.Restaurant).Take(request.MaxRestaurants).Select(i => i.ToResponse()).ToList(),
            Entertainments = selectedItems.Where(i => i.PlaceType == ItemType.Entertainment).Take(request.MaxEntertainments).Select(i => i.ToResponse()).ToList(),
            TourismAreas = selectedItems.Where(i => i.PlaceType == ItemType.TourismArea).Take(request.MaxTourismAreas).Select(i => i.ToResponse()).ToList()
        };

        return new Respond<TripPlanResponse>
        {
            Succeeded = true,
            Message = "Trip plan optimized successfully",
            Data = tripPlanResponse,
            Meta = new { TotalItems = selectedItems.Count }
        };
    }

    private (int accommodation, int food, int entertainment, int tourism) CalculatePriorities(Queue<string> interests)
    {
        int maxPriority = interests.Count; // أعلى أولوية هي عدد العناصر
        int accommodationPriority = 0, foodPriority = 0, entertainmentPriority = 0, tourismPriority = 0;

        while (interests.Count > 0)
        {
            var interest = interests.Dequeue();
            switch (interest.ToLower())
            {
                case "accommodation":
                    accommodationPriority = maxPriority--;
                    break;
                case "restaurants":
                case "food":
                    foodPriority = maxPriority--;
                    break;
                case "entertainments":
                case "entertainment":
                    entertainmentPriority = maxPriority--;
                    break;
                case "tourismareas":
                case "tourism":
                    tourismPriority = maxPriority--;
                    break;
            }
        }

        return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
    }
}


// HELPER CLASSES
public record TripPlanRequest(
    int ZoneId, 
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