using Moq;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Behaviors;
using MediatR;

namespace TripMinder.Tests
{
    public class TripPlanOptimizerTests
    {
        [Fact]
        public async Task OptimizePlan_ReturnsAccommodation_WhenAvailable()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var solverMock = new Mock<IKnapsackSolver>();
            var itemFetcherMock = new Mock<IItemFetcher>();

            var request = new TripPlanRequest(
                ZoneId: 1,
                BudgetPerAdult: 100,
                NumberOfTravelers: 2,
                Interests: new Queue<string>(new[] { "accommodation", "restaurants" }),
                MaxRestaurants: 3,
                MaxAccommodations: 1,
                MaxEntertainments: 2,
                MaxTourismAreas: 2
            );

            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Hotel A", AveragePricePerAdult = 50, Score = 10, PlaceType = ItemType.Accommodation },
                new Item { Id = 2, Name = "Restaurant B", AveragePricePerAdult = 30, Score = 8, PlaceType = ItemType.Restaurant }
            };

            itemFetcherMock.Setup(f => f.FetchItems(1, It.IsAny<(int, int, int, int)>(), mediatorMock.Object))
                .ReturnsAsync(items);

            var constraints = new UserDefinedKnapsackConstraints(3, 1, 2, 2);
            solverMock.Setup(s => s.GetMaxProfit(200, items, constraints))
                .Returns((18f, items));

            var optimizer = new TripPlanOptimizer(mediatorMock.Object, solverMock.Object, itemFetcherMock.Object);

            // Act
            var response = await optimizer.OptimizePlan(request);

            // Debug
            Console.WriteLine($"Selected Items Count: {response.Data?.Restaurants?.Count}");
            if (response.Data.Accommodation == null)
            {
                Console.WriteLine("Accommodation is null!");
                foreach (var item in items)
                {
                    Console.WriteLine($"Item: {item.Id}, Type: {item.PlaceType}");
                }
            }

            // Assert
            Assert.True(response.Succeeded);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.Accommodation); // فشل هنا
            Assert.Equal(1, response.Data.Accommodation.Id);
            Assert.Single(response.Data.Restaurants);
            Assert.Equal(2, response.Data.Restaurants[0].Id);
        }
    }
}