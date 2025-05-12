using MediatR;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Models;

namespace TripMinder.Core.Behaviors.Shared;

public class ItemFetcher : IItemFetcher
{
    private static readonly Random _random = new Random(12345);
    private readonly IMediator _mediator;

    public ItemFetcher(IMediator mediator)
    {
        this._mediator = mediator;
    }

    public async Task<List<Item>> FetchItems(int governorateId, int? zoneId, (int a, int f, int e, int t) priorities, double dailyBudgetPerAdult)
    {
        var allItems = new List<Item>();

        if (zoneId.HasValue)
        {
            await FetchItemsByZoneId(zoneId.Value, priorities, allItems, dailyBudgetPerAdult);
        }
        else
        {
            await FetchItemsByGovernorateId(governorateId, priorities, allItems, dailyBudgetPerAdult);
        }

        Console.WriteLine($"Fetched Items: Total={allItems.Count}, Restaurants={allItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={allItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={allItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={allItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", allItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        var filteredItems = allItems
            .Where(item =>
            {
                int priority = item.PlaceType switch
                {
                    ItemType.Accommodation => priorities.a,
                    ItemType.Restaurant => priorities.f,
                    ItemType.Entertainment => priorities.e,
                    ItemType.TourismArea => priorities.t,
                    _ => 0
                };
                return priority > 0;
            })
            .Select(item => { item.Score = Math.Max(item.Score, 0.01f); return item; })
            .OrderByDescending(item =>
            {
                int priority = item.PlaceType switch
                {
                    ItemType.Accommodation => priorities.a,
                    ItemType.Restaurant => priorities.f,
                    ItemType.Entertainment => priorities.e,
                    ItemType.TourismArea => priorities.t,
                    _ => 0
                };
                return priority == 0 ? 0 : priority * item.Score;
            })
            .ToList();

        // Fallback to all items if filteredItems is empty
        if (!filteredItems.Any() && allItems.Any())
        {
            Console.WriteLine("No items matched priorities, returning all items");
            filteredItems = allItems
                .Select(item => { item.Score = Math.Max(item.Score, 0.01f); return item; })
                .OrderByDescending(item => item.Score)
                .ToList();
        }

        Console.WriteLine($"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", filteredItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        return filteredItems;
    }

    private async Task FetchItemsByZoneId(int zoneId, (int a, int f, int e, int t) priorities, List<Item> allItems, double dailyBudgetPerAdult)
    {
        var accommodationsResponse = await _mediator.Send(new GetAccomodationsListByZoneIdQuery(zoneId, priorities.a));
        Console.WriteLine($"Accommodations Response: Succeeded={accommodationsResponse?.Succeeded}, Count={(accommodationsResponse?.Data?.Count() ?? 0)}");
        if (accommodationsResponse?.Succeeded == true && accommodationsResponse.Data != null)
        {
            allItems.AddRange(accommodationsResponse.Data.Select(a =>
            {
                var item = new Item
                {
                    Id = a.Id,
                    GlobalId = $"{ItemType.Accommodation}_{a.Id}",
                    Name = a.Name,
                    ClassType = a.ClassType,
                    AveragePricePerAdult = a.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Accommodation)
                        : (float)a.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(a.ClassType ?? "C", priorities.a, a.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.Accommodation,
                    Rating = a.Rating,
                    ImageSource = a.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Accommodations found for ZoneId: {zoneId}");
        }

        var restaurantsResponse = await _mediator.Send(new GetRestaurantsListByZoneIdQuery(zoneId, priorities.f));
        Console.WriteLine($"Restaurants Response: Succeeded={restaurantsResponse?.Succeeded}, Count={(restaurantsResponse?.Data?.Count() ?? 0)}");
        if (restaurantsResponse?.Succeeded == true && restaurantsResponse.Data != null)
        {
            var restaurantItems = restaurantsResponse.Data.Select(r =>
            {
                var item = new Item
                {
                    Id = r.Id,
                    GlobalId = $"{ItemType.Restaurant}_{r.Id}",
                    Name = r.Name,
                    ClassType = r.ClassType,
                    AveragePricePerAdult = r.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Restaurant)
                        : (float)r.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(r.ClassType ?? "C", priorities.f, r.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.Restaurant,
                    Rating = r.Rating,
                    ImageSource = r.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }).ToList();
            allItems.AddRange(restaurantItems);
            Console.WriteLine($"Restaurants Added: {restaurantItems.Count}");
        }
        else
        {
            Console.WriteLine($"No Restaurants found for ZoneId: {zoneId}");
        }

        var entertainmentsResponse = await _mediator.Send(new GetEntertainmentsListByZoneIdQuery(zoneId, priorities.e));
        Console.WriteLine($"Entertainments Response: Succeeded={entertainmentsResponse?.Succeeded}, Count={(entertainmentsResponse?.Data?.Count() ?? 0)}");
        if (entertainmentsResponse?.Succeeded == true && entertainmentsResponse.Data != null)
        {
            allItems.AddRange(entertainmentsResponse.Data.Select(e =>
            {
                var item = new Item
                {
                    Id = e.Id,
                    GlobalId = $"{ItemType.Entertainment}_{e.Id}",
                    Name = e.Name,
                    ClassType = e.ClassType,
                    AveragePricePerAdult = e.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Entertainment)
                        : (float)e.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(e.ClassType ?? "C", priorities.e, e.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.Entertainment,
                    Rating = e.Rating,
                    ImageSource = e.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Entertainments found for ZoneId: {zoneId}");
        }

        var tourismAreasResponse = await _mediator.Send(new GetTourismAreasListByZoneIdQuery(zoneId, priorities.t));
        Console.WriteLine($"TourismAreas Response: Succeeded={tourismAreasResponse?.Succeeded}, Count={(tourismAreasResponse?.Data?.Count() ?? 0)}");
        if (tourismAreasResponse?.Succeeded == true && tourismAreasResponse.Data != null)
        {
            allItems.AddRange(tourismAreasResponse.Data.Select(t =>
            {
                var item = new Item
                {
                    Id = t.Id,
                    GlobalId = $"{ItemType.TourismArea}_{t.Id}",
                    Name = t.Name,
                    ClassType = t.ClassType,
                    AveragePricePerAdult = t.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.TourismArea)
                        : (float)t.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(t.ClassType ?? "C", priorities.t, t.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.TourismArea,
                    Rating = t.Rating,
                    ImageSource = t.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No TourismAreas found for ZoneId: {zoneId}");
        }
    }

    private async Task FetchItemsByGovernorateId(int governorateId, (int a, int f, int e, int t) priorities, List<Item> allItems, double dailyBudgetPerAdult)
    {
        var accommodationsResponse = await _mediator.Send(new GetAccomodationsListByGovernorateIdQuery(governorateId, priorities.a));
        Console.WriteLine($"Accommodations Response: Succeeded={accommodationsResponse?.Succeeded}, Count={(accommodationsResponse?.Data?.Count() ?? 0)}");
        if (accommodationsResponse?.Succeeded == true && accommodationsResponse.Data != null)
        {
            allItems.AddRange(accommodationsResponse.Data.Select(a =>
            {
                var item = new Item
                {
                    Id = a.Id,
                    GlobalId = $"{ItemType.Accommodation}_{a.Id}",
                    Name = a.Name,
                    ClassType = a.ClassType,
                    AveragePricePerAdult = a.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Accommodation)
                        : (float)a.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(a.ClassType ?? "C", priorities.a, a.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.Accommodation,
                    Rating = a.Rating,
                    ImageSource = a.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Accommodations found for GovernorateId: {governorateId}");
        }

        var restaurantsResponse = await _mediator.Send(new GetRestaurantsListByGovernorateIdQuery(governorateId, priorities.f));
        Console.WriteLine($"Restaurants Response: Succeeded={restaurantsResponse?.Succeeded}, Count={(restaurantsResponse?.Data?.Count() ?? 0)}");
        if (restaurantsResponse?.Succeeded == true && restaurantsResponse.Data != null)
        {
            var restaurantItems = restaurantsResponse.Data.Select(r =>
            {
                var item = new Item
                {
                    Id = r.Id,
                    GlobalId = $"{ItemType.Restaurant}_{r.Id}",
                    Name = r.Name,
                    ClassType = r.ClassType,
                    AveragePricePerAdult = r.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Restaurant)
                        : (float)r.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(r.ClassType ?? "C", priorities.f, r.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.Restaurant,
                    Rating = r.Rating,
                    ImageSource = r.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }).ToList();
            allItems.AddRange(restaurantItems);
            Console.WriteLine($"Restaurants Added: {restaurantItems.Count}");
        }
        else
        {
            Console.WriteLine($"No Restaurants found for GovernorateId: {governorateId}");
        }

        var entertainmentsResponse = await _mediator.Send(new GetEntertainmentsListByGovernorateIdQuery(governorateId, priorities.e));
        Console.WriteLine($"Entertainments Response: Succeeded={entertainmentsResponse?.Succeeded}, Count={(entertainmentsResponse?.Data?.Count() ?? 0)}");
        if (entertainmentsResponse?.Succeeded == true && entertainmentsResponse.Data != null)
        {
            allItems.AddRange(entertainmentsResponse.Data.Select(e =>
            {
                var item = new Item
                {
                    Id = e.Id,
                    GlobalId = $"{ItemType.Entertainment}_{e.Id}",
                    Name = e.Name,
                    ClassType = e.ClassType,
                    AveragePricePerAdult = e.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.Entertainment)
                        : (float)e.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(e.ClassType ?? "C", priorities.e, e.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.Entertainment,
                    Rating = e.Rating,
                    ImageSource = e.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No Entertainments found for GovernorateId: {governorateId}");
        }

        var tourismAreasResponse = await _mediator.Send(new GetTourismAreasListByGovernorateIdQuery(governorateId, priorities.t));
        Console.WriteLine($"TourismAreas Response: Succeeded={tourismAreasResponse?.Succeeded}, Count={(tourismAreasResponse?.Data?.Count() ?? 0)}");
        if (tourismAreasResponse?.Succeeded == true && tourismAreasResponse.Data != null)
        {
            allItems.AddRange(tourismAreasResponse.Data.Select(t =>
            {
                var item = new Item
                {
                    Id = t.Id,
                    GlobalId = $"{ItemType.TourismArea}_{t.Id}",
                    Name = t.Name,
                    ClassType = t.ClassType,
                    AveragePricePerAdult = t.AveragePricePerAdult <= 0
                        ? GetRandomPrice(ItemType.TourismArea)
                        : (float)t.AveragePricePerAdult,
                    Score = CalculateScoreBehavior.CalculateScore(t.ClassType ?? "C", priorities.t, t.AveragePricePerAdult, dailyBudgetPerAdult),
                    PlaceType = ItemType.TourismArea,
                    Rating = t.Rating,
                    ImageSource = t.ImageUrl
                };
                Console.WriteLine($"Item: {item.Name}, GlobalId: {item.GlobalId}, Type: {item.PlaceType}, Price: {item.AveragePricePerAdult}, Score: {item.Score}");
                return item;
            }));
        }
        else
        {
            Console.WriteLine($"No TourismAreas found for GovernorateId: {governorateId}");
        }
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