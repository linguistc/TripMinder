using MediatR;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Behaviors.Knapsack;

public class TripPlanOptimizer
{
    private readonly IMediator _mediator;
    private readonly IKnapsackSolver _solver;

    public TripPlanOptimizer(IMediator mediator, IKnapsackSolver solver)
    {
        this._mediator = mediator;
        this._solver = solver;
        
    }

    public async Task<Respond<TripPlanResponse>> OptimizePlan(TripPlanRequest request)
    {
        // تحويل الـ Interests لـ Priorities
        var (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority) = CalculatePriorities(request.Interests);

        // جيب البيانات باستخدام Queries
        var accommodationsResponse = await _mediator.Send(new GetAccomodationsListByZoneIdQuery(request.ZoneId, accommodationPriority));
        var restaurantsResponse = await _mediator.Send(new GetRestaurantsListByZoneIdQuery(request.ZoneId, foodPriority));
        var entertainmentsResponse = await _mediator.Send(new GetEntertainmentsListByZoneIdQuery(request.ZoneId, entertainmentPriority));
        var tourismAreasResponse = await _mediator.Send(new GetTourismAreasListByZoneIdQuery(request.ZoneId, tourismPriority));

        // تحويل البيانات لـ Items للألجورزم
        var allItems = new List<TripMinder.Core.Behaviors.Knapsack.Item>();

        if (accommodationsResponse?.Succeeded == true && accommodationsResponse.Data != null)
        {
            allItems.AddRange(accommodationsResponse.Data.Select(a => new TripMinder.Core.Behaviors.Knapsack.Item
            {
                Id = a.Id,
                Name = a.Name,
                AveragePricePerAdult = (float)a.AveragePricePerAdult,
                Score = a.Score,
                PlaceType= ItemType.Accommodation
            }));
        }

        if (restaurantsResponse?.Succeeded == true && restaurantsResponse.Data != null)
        {
            allItems.AddRange(restaurantsResponse.Data.Select(r => new TripMinder.Core.Behaviors.Knapsack.Item
            {
                Id = r.Id,
                Name = r.Name,
                AveragePricePerAdult = (float)r.AveragePricePerAdult,
                Score = r.Score,
                PlaceType= ItemType.Restaurant
            }));
        }

        if (entertainmentsResponse?.Succeeded == true && entertainmentsResponse.Data != null)
        {
            allItems.AddRange(entertainmentsResponse.Data.Select(e => new TripMinder.Core.Behaviors.Knapsack.Item
            {
                Id = e.Id,
                Name = e.Name,
                AveragePricePerAdult = (float)e.AveragePricePerAdult,
                Score = e.Score,
                PlaceType= ItemType.Entertainment
            }));
        }

        if (tourismAreasResponse?.Succeeded == true && tourismAreasResponse.Data != null)
        {
            allItems.AddRange(tourismAreasResponse.Data.Select(t => new TripMinder.Core.Behaviors.Knapsack.Item
            {
                Id = t.Id,
                Name = t.Name,
                AveragePricePerAdult = (float)t.AveragePricePerAdult,
                Score = t.Score,
                PlaceType= ItemType.TourismArea
            }));
        }

        // تطبيق الألجورزم
        var totalBudget = request.BudgetPerAdult * request.NumberOfTravelers;
        var (maxProfit, selectedItems) = this._solver.GetMaxProfit((int)totalBudget, allItems);

        // تحويل النتيجة لـ Response
        var tripPlanResponse = new TripPlanResponse
        {
            Accommodation = selectedItems.FirstOrDefault(i => i.PlaceType == ItemType.Accommodation)?.ToResponse(),
            Restaurants = selectedItems.Where(i => i.PlaceType == ItemType.Restaurant).Take(3).Select(i => i.ToResponse()).ToList(),
            Entertainments = selectedItems.Where(i => i.PlaceType == ItemType.Entertainment).Take(3).Select(i => i.ToResponse()).ToList(),
            TourismAreas = selectedItems.Where(i => i.PlaceType == ItemType.TourismArea).Take(3).Select(i => i.ToResponse()).ToList()
        };

        return new Respond<TripPlanResponse>
        {
            Succeeded = true,
            Message = "Trip plan optimized Succeededfully",
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
public record TripPlanRequest(int ZoneId, double BudgetPerAdult, int NumberOfTravelers, Queue<string> Interests);

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