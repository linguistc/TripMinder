using MediatR;
using Moq;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Responses;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Responses;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
using Xunit.Abstractions;

namespace TripMinder.Core.Tests.Behaviors.Knapsack;

public class ItemFetcherTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly IItemFetcher _fetcher;
    private readonly ITestOutputHelper _output;

    public ItemFetcherTests(ITestOutputHelper output)
    {
        _mockMediator = new Mock<IMediator>();
        _fetcher = new ItemFetcher(_mockMediator.Object);
        _output = output;
    }

    private List<Item> CreateStandardTestItems()
    {
        return new List<Item>
        {
            new Item { Id = 1, Name = "Restaurant1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 200, Score = 0, ClassType = "A", Rating = 4.5f, ImageSource = "img1" },
            new Item { Id = 2, Name = "Accommodation1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 500, Score = 0, ClassType = "B", Rating = 4.8f, ImageSource = "img2" },
            new Item { Id = 3, Name = "Entertainment1", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 150, Score = 0, ClassType = "C", Rating = 4.0f, ImageSource = "img3" },
            new Item { Id = 4, Name = "TourismArea1", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 100, Score = 0, ClassType = "A", Rating = 4.2f, ImageSource = "img4" },
            new Item { Id = 5, Name = "Restaurant2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 300, Score = 0, ClassType = "B", Rating = 4.3f, ImageSource = "img5" }
        };
    }

    [Fact]
    public async Task FetchItems_GivenZeroPriorities_ReturnsAllItemsSortedByScoreToPriceRatio()
    {
        // Arrange
        int governorateId = 1;
        int? zoneId = null;
        var priorities = (a: 0, f: 0, e: 0, t: 0);
        var items = CreateStandardTestItems();
        var accommodations = items.Where(i => i.PlaceType == ItemType.Accommodation).Select(i => new GetAccomodationsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();
        var restaurants = items.Where(i => i.PlaceType == ItemType.Restaurant).Select(i => new GetRestaurantsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();
        var entertainments = items.Where(i => i.PlaceType == ItemType.Entertainment).Select(i => new GetEntertainmentsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();
        var tourismAreas = items.Where(i => i.PlaceType == ItemType.TourismArea).Select(i => new GetTourismAreasListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetAccomodationsListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetAccomodationsListResponse>> { Succeeded = true, Data = accommodations, Meta = new { Count = accommodations.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetRestaurantsListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetRestaurantsListResponse>> { Succeeded = true, Data = restaurants, Meta = new { Count = restaurants.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetEntertainmentsListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetEntertainmentsListResponse>> { Succeeded = true, Data = entertainments, Meta = new { Count = entertainments.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetTourismAreasListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetTourismAreasListResponse>> { Succeeded = true, Data = tourismAreas, Meta = new { Count = tourismAreas.Count } });

        // Act
        var result = await _fetcher.FetchItems(governorateId, zoneId, priorities);

        // Assert
        Assert.Equal(5, result.Count);
        
        var expectedItems = items.OrderBy(i => i.PlaceType switch
        {
            ItemType.Accommodation => 1,
            ItemType.Restaurant => 2,
            ItemType.Entertainment => 3,
            ItemType.TourismArea => 4,
            _ => 5
        }).ThenBy(i => i.Id).ToList();
        
        _output.WriteLine("Expected Order:");
        for (int i = 0; i < expectedItems.Count; i++)
        {
            expectedItems[i].Score = CalculateScoreBehavior.CalculateScore(expectedItems[i].ClassType ?? "C", GetPriority(expectedItems[i].PlaceType, priorities), expectedItems[i].AveragePricePerAdult);
            _output.WriteLine($"Expected Item {i + 1}: {expectedItems[i].Name}, Type: {expectedItems[i].PlaceType}, Score: {expectedItems[i].Score}, Price: {expectedItems[i].AveragePricePerAdult}, Priority*Score: {GetPriority(expectedItems[i].PlaceType, priorities) * expectedItems[i].Score}");
        }

        _output.WriteLine("Actual Order:");
        for (int i = 0; i < result.Count; i++)
        {
            _output.WriteLine($"Actual Item {i + 1}: {result[i].Name}, Type: {result[i].PlaceType}, Score: {result[i].Score}, Price: {result[i].AveragePricePerAdult}, Priority*Score: {GetPriority(result[i].PlaceType, priorities) * result[i].Score}");
        }

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(expectedItems[i].Id, result[i].Id);
            Assert.Equal(0, result[i].Score); // All scores should be 0 since priorities are 0
        }
        _output.WriteLine($"Total Items Fetched: {result.Count}");
        
    }

    
    [Fact]
    public async Task FetchItems_GivenMaxPriorityAccommodation_ReturnsAccommodationItemsFirst()
    {
        // Arrange
        int governorateId = 1;
        int? zoneId = 2;
        var priorities = (a: 5, f: 0, e: 0, t: 0);
        var items = CreateStandardTestItems();
        var accommodations = items.Where(i => i.PlaceType == ItemType.Accommodation).Select(i => new GetAccomodationsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();
        var restaurants = items.Where(i => i.PlaceType == ItemType.Restaurant).Select(i => new GetRestaurantsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();
        var entertainments = items.Where(i => i.PlaceType == ItemType.Entertainment).Select(i => new GetEntertainmentsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();
        var tourismAreas = items.Where(i => i.PlaceType == ItemType.TourismArea).Select(i => new GetTourismAreasListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetAccomodationsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetAccomodationsListResponse>> { Succeeded = true, Data = accommodations, Meta = new { Count = accommodations.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetRestaurantsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetRestaurantsListResponse>> { Succeeded = true, Data = restaurants, Meta = new { Count = restaurants.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetEntertainmentsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetEntertainmentsListResponse>> { Succeeded = true, Data = entertainments, Meta = new { Count = entertainments.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetTourismAreasListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetTourismAreasListResponse>> { Succeeded = true, Data = tourismAreas, Meta = new { Count = tourismAreas.Count } });

        // Act
        var result = await _fetcher.FetchItems(governorateId, zoneId, priorities);

        // Assert
        Assert.Equal(5, result.Count);
        Assert.True(result.Take(1).All(i => i.PlaceType == ItemType.Accommodation));
        // Calculate expected scores for sorting
        var expectedItems = items.Select(i =>
        {
            var priority = GetPriority(i.PlaceType, priorities);
            i.Score = CalculateScoreBehavior.CalculateScore(i.ClassType ?? "C", priority, i.AveragePricePerAdult);
            return i;
        }).OrderByDescending(i => GetPriority(i.PlaceType, priorities) * i.Score)
          .ThenBy(i => i.PlaceType switch
          {
              ItemType.Accommodation => 1,
              ItemType.Restaurant => 2,
              ItemType.Entertainment => 3,
              ItemType.TourismArea => 4,
              _ => 5
          }).ThenBy(i => i.Id).ToList();

        _output.WriteLine("Expected Order:");
        for (int i = 0; i < expectedItems.Count; i++)
        {
            _output.WriteLine($"Expected Item {i + 1}: {expectedItems[i].Name}, Type: {expectedItems[i].PlaceType}, Score: {expectedItems[i].Score}, Price: {expectedItems[i].AveragePricePerAdult}, Priority*Score: {GetPriority(expectedItems[i].PlaceType, priorities) * expectedItems[i].Score}");
        }

        _output.WriteLine("Actual Order:");
        for (int i = 0; i < result.Count; i++)
        {
            _output.WriteLine($"Actual Item {i + 1}: {result[i].Name}, Type: {result[i].PlaceType}, Score: {result[i].Score}, Price: {result[i].AveragePricePerAdult}, Priority*Score: {GetPriority(result[i].PlaceType, priorities) * result[i].Score}");
        }

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(expectedItems[i].Id, result[i].Id);
            if (i == 0)
                Assert.Equal(85, result[i].Score); // Accommodation1 (ClassType: B, priority: 5) -> 17 * 5 = 85
            else
                Assert.Equal(0, result[i].Score); // Other items have priority 0
        }
        _output.WriteLine($"Total Items Fetched: {result.Count}");
    }
    
    
    [Fact]
    public async Task FetchItems_GivenInvalidZoneId_ReturnsEmptyList()
    {
        // Arrange
        int governorateId = 1;
        int? zoneId = -1;
        var priorities = (a: 1, f: 1, e: 1, t: 1);
        _mockMediator.Setup(m => m.Send(It.IsAny<GetAccomodationsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetAccomodationsListResponse>> { Succeeded = false, Data = null });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetRestaurantsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetRestaurantsListResponse>> { Succeeded = false, Data = null });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetEntertainmentsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetEntertainmentsListResponse>> { Succeeded = false, Data = null });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetTourismAreasListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetTourismAreasListResponse>> { Succeeded = false, Data = null });

        // Act
        var result = await _fetcher.FetchItems(governorateId, zoneId, priorities);

        // Assert
        Assert.Empty(result);
        _output.WriteLine($"Total Items Fetched: {result.Count}");
    }

    [Fact]
    public async Task FetchItems_GivenNullMediatorResponseForRestaurants_ReturnsOtherTypesSorted()
    {
        // Arrange
        int governorateId = 1;
        int? zoneId = null;
        var priorities = (a: 2, f: 2, e: 2, t: 2);
        var items = CreateStandardTestItems().Where(i => i.PlaceType != ItemType.Restaurant).ToList();
        var accommodations = items.Where(i => i.PlaceType == ItemType.Accommodation).Select(i => new GetAccomodationsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();
        var entertainments = items.Where(i => i.PlaceType == ItemType.Entertainment).Select(i => new GetEntertainmentsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();
        var tourismAreas = items.Where(i => i.PlaceType == ItemType.TourismArea).Select(i => new GetTourismAreasListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0 
        }).ToList();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetAccomodationsListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetAccomodationsListResponse>> { Succeeded = true, Data = accommodations, Meta = new { Count = accommodations.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetRestaurantsListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetRestaurantsListResponse>> { Succeeded = false, Data = null });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetEntertainmentsListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetEntertainmentsListResponse>> { Succeeded = true, Data = entertainments, Meta = new { Count = entertainments.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetTourismAreasListByGovernorateIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetTourismAreasListResponse>> { Succeeded = true, Data = tourismAreas, Meta = new { Count = tourismAreas.Count } });

        // Act
        var result = await _fetcher.FetchItems(governorateId, zoneId, priorities);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.DoesNotContain(result, i => i.PlaceType == ItemType.Restaurant);
        // Calculate expected scores for sorting
        var expectedItems = items.Select(i =>
        {
            var priority = GetPriority(i.PlaceType, priorities);
            i.Score = CalculateScoreBehavior.CalculateScore(i.ClassType, priority, i.AveragePricePerAdult);
            return i;
        }).OrderByDescending(i => GetPriority(i.PlaceType, priorities) * i.Score).ToList();

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(expectedItems[i].Id, result[i].Id);
            _output.WriteLine($"Item {i + 1}: {result[i].Name}, Type: {result[i].PlaceType}, Score: {result[i].Score}, Price: {result[i].AveragePricePerAdult}");
        }
        _output.WriteLine($"Total Items Fetched: {result.Count}");
    }

    [Fact]
    public async Task FetchItems_GivenMaxPriorityForNonExistentAccommodation_ReturnsOtherItemsSorted()
    {
        // Arrange
        int governorateId = 1;
        int? zoneId = 2;
        var priorities = (a: 5, f: 0, e: 0, t: 0);
        var items = CreateStandardTestItems().Where(i => i.PlaceType != ItemType.Accommodation).ToList();
        var restaurants = items.Where(i => i.PlaceType == ItemType.Restaurant).Select(i => new GetRestaurantsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();
        var entertainments = items.Where(i => i.PlaceType == ItemType.Entertainment).Select(i => new GetEntertainmentsListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();
        var tourismAreas = items.Where(i => i.PlaceType == ItemType.TourismArea).Select(i => new GetTourismAreasListResponse
        {
            Id = i.Id,
            Name = i.Name,
            ClassType = i.ClassType,
            AveragePricePerAdult = i.AveragePricePerAdult,
            Rating = i.Rating,
            ImageSource = i.ImageSource,
            Score = 0
        }).ToList();

        _mockMediator.Setup(m => m.Send(It.IsAny<GetAccomodationsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetAccomodationsListResponse>> { Succeeded = false, Data = null });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetRestaurantsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetRestaurantsListResponse>> { Succeeded = true, Data = restaurants, Meta = new { Count = restaurants.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetEntertainmentsListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetEntertainmentsListResponse>> { Succeeded = true, Data = entertainments, Meta = new { Count = entertainments.Count } });
        _mockMediator.Setup(m => m.Send(It.IsAny<GetTourismAreasListByZoneIdQuery>(), default))
            .ReturnsAsync(new Respond<List<GetTourismAreasListResponse>> { Succeeded = true, Data = tourismAreas, Meta = new { Count = tourismAreas.Count } });

        // Act
        var result = await _fetcher.FetchItems(governorateId, zoneId, priorities);

        // Assert
        Assert.Equal(4, result.Count); 
        Assert.DoesNotContain(result, i => i.PlaceType == ItemType.Accommodation);
        // Calculate expected scores for sorting
        var expectedItems = items.Select(i =>
        {
            var priority = GetPriority(i.PlaceType, priorities);
            i.Score = CalculateScoreBehavior.CalculateScore(i.ClassType ?? "C", priority, i.AveragePricePerAdult);
            return i;
        }).OrderBy(i => i.PlaceType switch
        {
            ItemType.Restaurant => 1,
            ItemType.Entertainment => 2,
            ItemType.TourismArea => 3,
            _ => 4
        }).ThenBy(i => i.Id).ToList();

        _output.WriteLine("Expected Order:");
        for (int i = 0; i < expectedItems.Count; i++)
        {
            _output.WriteLine($"Expected Item {i + 1}: {expectedItems[i].Name}, Type: {expectedItems[i].PlaceType}, Score: {expectedItems[i].Score}, Price: {expectedItems[i].AveragePricePerAdult}, Priority*Score: {GetPriority(expectedItems[i].PlaceType, priorities) * expectedItems[i].Score}");
        }

        _output.WriteLine("Actual Order:");
        for (int i = 0; i < result.Count; i++)
        {
            _output.WriteLine($"Actual Item {i + 1}: {result[i].Name}, Type: {result[i].PlaceType}, Score: {result[i].Score}, Price: {result[i].AveragePricePerAdult}, Priority*Score: {GetPriority(result[i].PlaceType, priorities) * result[i].Score}");
        }

        for (int i = 0; i < result.Count; i++)
        {
            Assert.Equal(expectedItems[i].Id, result[i].Id);
            Assert.Equal(0, result[i].Score); 
        }
        _output.WriteLine($"Total Items Fetched: {result.Count}");
    }

    private int GetPriority(ItemType type, (int a, int f, int e, int t) priorities)
    {
        return type switch
        {
            ItemType.Accommodation => priorities.a,
            ItemType.Restaurant => priorities.f,
            ItemType.Entertainment => priorities.e,
            ItemType.TourismArea => priorities.t,
            _ => 0
        };
    }
}
    
