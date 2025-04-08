using MediatR;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Models;

namespace TripMinder.Core.Behaviors.Knapsack;

public class ItemFetcher : IItemFetcher
{
    public async Task<List<Item>> FetchItems(int zoneId, (int a, int f, int e, int t) priorities, IMediator mediator)
    {
        var allItems = new List<Item>();
        
        var accommodationsResponse = await mediator.Send(new GetAccomodationsListByZoneIdQuery(zoneId, priorities.a));
        if (accommodationsResponse?.Succeeded == true && accommodationsResponse.Data != null)
            allItems.AddRange(accommodationsResponse.Data.Select(a => new Item
            {
                Id = a.Id,
                Name = a.Name,
                AveragePricePerAdult = (float)a.AveragePricePerAdult,
                Score = a.Score,
                PlaceType = ItemType.Accommodation
            }));

        var restaurantsResponse = await mediator.Send(new GetRestaurantsListByZoneIdQuery(zoneId, priorities.f));
        if (restaurantsResponse?.Succeeded == true && restaurantsResponse.Data != null)
            allItems.AddRange(restaurantsResponse.Data.Select(r => new Item
            {
                Id = r.Id,
                Name = r.Name,
                AveragePricePerAdult = (float)r.AveragePricePerAdult,
                Score = r.Score,
                PlaceType = ItemType.Restaurant
            }));
        
        var entertainmentsResponse = await mediator.Send(new GetEntertainmentsListByZoneIdQuery(zoneId, priorities.e));
        if (entertainmentsResponse?.Succeeded == true && entertainmentsResponse.Data != null)
            allItems.AddRange(entertainmentsResponse.Data.Select(e => new Item
            {
                Id = e.Id,
                Name = e.Name,
                AveragePricePerAdult = (float)e.AveragePricePerAdult,
                Score = e.Score,
                PlaceType = ItemType.Entertainment
            }));

        var tourismAreasResponse = await mediator.Send(new GetTourismAreasListByZoneIdQuery(zoneId, priorities.t));
        if (tourismAreasResponse?.Succeeded == true && tourismAreasResponse.Data != null)
            allItems.AddRange(tourismAreasResponse.Data.Select(t => new Item
            {
                Id = t.Id,
                Name = t.Name,
                AveragePricePerAdult = (float)t.AveragePricePerAdult,
                Score = t.Score,
                PlaceType = ItemType.TourismArea
            }));

        return allItems;
    }
}