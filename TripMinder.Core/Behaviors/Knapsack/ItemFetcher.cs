using MediatR;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Behaviors;

namespace TripMinder.Core.Behaviors.Knapsack;

public class ItemFetcher : IItemFetcher
{
    private static readonly Random _random = new Random(12345);

    public async Task<List<Item>> FetchItems(int zoneId, (int a, int f, int e, int t) priorities, IMediator mediator)
    {
        var allItems = new List<Item>();

        var accommodationsResponse = await mediator.Send(new GetAccomodationsListByZoneIdQuery(zoneId, priorities.a));
        if (accommodationsResponse?.Succeeded == true && accommodationsResponse.Data != null)
        {
            allItems.AddRange(accommodationsResponse.Data.Select(a =>
            {
                var item = new Item
                {
                    Id = a.Id,
                    Name = a.Name,
                    AveragePricePerAdult = a.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Accommodation)
                        : (float)a.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(a.ClassType ?? "C", priorities.a),
                    PlaceType = ItemType.Accommodation
                };
                Console.WriteLine($"Item: {item.Name}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }

        var restaurantsResponse = await mediator.Send(new GetRestaurantsListByZoneIdQuery(zoneId, priorities.f));
        if (restaurantsResponse?.Succeeded == true && restaurantsResponse.Data != null)
        {
            allItems.AddRange(restaurantsResponse.Data.Select(r => 
            {
                var item = new Item
                {
                    Id = r.Id,
                    Name = r.Name,
                    AveragePricePerAdult = r.AveragePricePerAdult <= 0 ? GetRandomPrice(ItemType.Restaurant) : (float)r.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(r.ClassType ?? "C", priorities.f),
                    PlaceType = ItemType.Restaurant
                };
                Console.WriteLine($"Item: {item.Name}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }

        var entertainmentsResponse = await mediator.Send(new GetEntertainmentsListByZoneIdQuery(zoneId, priorities.e));
        if (entertainmentsResponse?.Succeeded == true && entertainmentsResponse.Data != null)
        {
            allItems.AddRange(entertainmentsResponse.Data.Select(e => 
            {
                var item = new Item
                {
                    Id = e.Id,
                    Name = e.Name,
                    AveragePricePerAdult = e.AveragePricePerAdult <= 0 ? GetRandomPrice(ItemType.Entertainment) : (float)e.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(e.ClassType ?? "C", priorities.e),
                    PlaceType = ItemType.Entertainment
                };
                Console.WriteLine($"Item: {item.Name}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }

        var tourismAreasResponse = await mediator.Send(new GetTourismAreasListByZoneIdQuery(zoneId, priorities.t));
        if (tourismAreasResponse?.Succeeded == true && tourismAreasResponse.Data != null)
        {
            allItems.AddRange(tourismAreasResponse.Data.Select(t => 
            {
                var item = new Item
                {
                    Id = t.Id,
                    Name = t.Name,
                    AveragePricePerAdult = t.AveragePricePerAdult <= 0 ? GetRandomPrice(ItemType.TourismArea) : (float)t.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(t.ClassType ?? "C", priorities.t),
                    PlaceType = ItemType.TourismArea
                };
                Console.WriteLine($"Item: {item.Name}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }

        return allItems;
    }

    private float GetRandomPrice(ItemType type)
    {
        return type switch
        {
            ItemType.Accommodation => _random.Next(500, 2000), // نطاق واقعي للسكن
            ItemType.Restaurant => _random.Next(100, 800),     // نطاق واقعي للمطاعم
            ItemType.Entertainment => _random.Next(50, 400),   // نطاق واقعي للترفيه
            ItemType.TourismArea => _random.Next(50, 300),     // نطاق واقعي للمناطق السياحية
            _ => _random.Next(50, 500)
        };
    }
}