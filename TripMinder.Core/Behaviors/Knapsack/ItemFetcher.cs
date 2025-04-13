using MediatR;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Models;

namespace TripMinder.Core.Behaviors.Knapsack;

public class ItemFetcher : IItemFetcher
{
    private static readonly Random _random = new Random(12345);

    public async Task<List<Item>> FetchItems(int zoneId, (int a, int f, int e, int t) priorities, IMediator mediator)
    {
        var allItems = new List<Item>();

        // جلب الإقامات
        var accommodationsResponse = await mediator.Send(new GetAccomodationsListByZoneIdQuery(zoneId, priorities.a));
        Console.WriteLine($"Accommodations Response: Succeeded={accommodationsResponse?.Succeeded}, Count={(accommodationsResponse?.Data?.Count() ?? 0)}");
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
                    Score = CalculateScoreBehavior.CalculateScore(a.ClassType ?? "C", priorities.a, a.AveragePricePerAdult),
                    PlaceType = ItemType.Accommodation
                };
                Console.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Accommodations found for ZoneId: {zoneId}");
        }

        // جلب المطاعم
        var restaurantsResponse = await mediator.Send(new GetRestaurantsListByZoneIdQuery(zoneId, priorities.f));
        Console.WriteLine($"Restaurants Response: Succeeded={restaurantsResponse?.Succeeded}, Count={(restaurantsResponse?.Data?.Count() ?? 0)}");
        if (restaurantsResponse?.Succeeded == true && restaurantsResponse.Data != null)
        {
            allItems.AddRange(restaurantsResponse.Data.Select(r =>
            {
                var item = new Item
                {
                    Id = r.Id,
                    Name = r.Name,
                    AveragePricePerAdult = r.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Restaurant)
                        : (float)r.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(r.ClassType ?? "C", priorities.f, r.AveragePricePerAdult),
                    PlaceType = ItemType.Restaurant
                };
                Console.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Restaurants found for ZoneId: {zoneId}");
        }

        // جلب الترفيه
        var entertainmentsResponse = await mediator.Send(new GetEntertainmentsListByZoneIdQuery(zoneId, priorities.e));
        Console.WriteLine($"Entertainments Response: Succeeded={entertainmentsResponse?.Succeeded}, Count={(entertainmentsResponse?.Data?.Count() ?? 0)}");
        if (entertainmentsResponse?.Succeeded == true && entertainmentsResponse.Data != null)
        {
            allItems.AddRange(entertainmentsResponse.Data.Select(e =>
            {
                var item = new Item
                {
                    Id = e.Id,
                    Name = e.Name,
                    AveragePricePerAdult = e.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Entertainment)
                        : (float)e.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(e.ClassType ?? "C", priorities.e, e.AveragePricePerAdult),
                    PlaceType = ItemType.Entertainment
                };
                Console.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Entertainments found for ZoneId: {zoneId}");
        }

        // جلب المناطق السياحية
        var tourismAreasResponse = await mediator.Send(new GetTourismAreasListByZoneIdQuery(zoneId, priorities.t));
        Console.WriteLine($"TourismAreas Response: Succeeded={tourismAreasResponse?.Succeeded}, Count={(tourismAreasResponse?.Data?.Count() ?? 0)}");
        if (tourismAreasResponse?.Succeeded == true && tourismAreasResponse.Data != null)
        {
            allItems.AddRange(tourismAreasResponse.Data.Select(t =>
            {
                var item = new Item
                {
                    Id = t.Id,
                    Name = t.Name,
                    AveragePricePerAdult = t.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.TourismArea)
                        : (float)t.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(t.ClassType ?? "C", priorities.t, t.AveragePricePerAdult),
                    PlaceType = ItemType.TourismArea
                };
                Console.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No TourismAreas found for ZoneId: {zoneId}");
        }

        Console.WriteLine($"Total Items Fetched: {allItems.Count}");
        return allItems.OrderByDescending(item =>
        {
            int priority = item.PlaceType switch
            {
                ItemType.Accommodation => priorities.a,
                ItemType.Restaurant => priorities.f,
                ItemType.Entertainment => priorities.e,
                ItemType.TourismArea => priorities.t,
                _ => 0
            };
            return priority * item.Score;
        }).ToList();
    }
    private float GetRandomPrice(ItemType type)
    {
        return type switch
        {
            ItemType.Accommodation => _random.Next(500, 2000),
            ItemType.Restaurant => _random.Next(100, 800),
            ItemType.Entertainment => _random.Next(50, 400),
            ItemType.TourismArea => _random.Next(50, 300),
            _ => _random.Next(50, 500)
        };
    }
}