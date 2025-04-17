using System;
using System.Collections.Generic;
using TripMinder.Core.Behaviors;
using TripMinder.Core.Behaviors.Knapsack;
using Xunit;
using Xunit.Abstractions;

namespace TripMinder.Core.Tests.Behaviors.Knapsack;

public class DynamicProgrammingCalculatorTests
{
    private readonly IDynamicProgrammingCalculator _calculator;
    private readonly ITestOutputHelper _output;

    public DynamicProgrammingCalculatorTests(ITestOutputHelper output)
    {
        _calculator = new DynamicProgrammingCalculator();
        _output = output;
    }

    private List<Item> CreateStandardTestItems()
    {
        return new List<Item>
        {
            new Item { Id = 1, Name = "Restaurant1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 200, Score = CalculateScoreBehavior.CalculateScore("A", 5, 200), ClassType = "A", Rating = 4.5, ImageSource = "img1" },
            new Item { Id = 2, Name = "Accommodation1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 500, Score = CalculateScoreBehavior.CalculateScore("B", 5, 500), ClassType = "B", Rating = 4.8, ImageSource = "img2" },
            new Item { Id = 3, Name = "Entertainment1", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 150, Score = CalculateScoreBehavior.CalculateScore("C", 5, 150), ClassType = "C", Rating = 4.0, ImageSource = "img3" },
            new Item { Id = 4, Name = "TourismArea1", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 100, Score = CalculateScoreBehavior.CalculateScore("A", 5, 100), ClassType = "A", Rating = 4.2, ImageSource = "img4" },
            new Item { Id = 5, Name = "Restaurant2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 300, Score = CalculateScoreBehavior.CalculateScore("B", 5, 300), ClassType = "B", Rating = 4.3, ImageSource = "img5" }
        };
    }

    private List<Item> CreateLargeTestItems(int count = 100)
    {
        var items = new List<Item>();
        for (int i = 0; i < count; i++)
        {
            string classType = i % 3 == 0 ? "A" : i % 3 == 1 ? "B" : "C";
            items.Add(new Item
            {
                Id = i + 1,
                Name = $"Item{i + 1}",
                PlaceType = (ItemType)(i % 4),
                AveragePricePerAdult = 100 + (i % 10) * 50,
                Score = CalculateScoreBehavior.CalculateScore(classType, 5, 100 + (i % 10) * 50),
                ClassType = classType,
                Rating = 4.0 + (i % 5) * 0.1,
                ImageSource = $"img{i + 1}"
            });
        }
        return items;
    }

    private List<Item> CreateItemsForHighBudget()
    {
        return new List<Item>
        {
            // Accommodations
            new Item { Id = 1, Name = "Luxury Hotel", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 2000, Score = CalculateScoreBehavior.CalculateScore("A", 5, 2000), ClassType = "A", Rating = 4.9, ImageSource = "img1" },
            new Item { Id = 2, Name = "Budget Hotel", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 500, Score = CalculateScoreBehavior.CalculateScore("C", 5, 500), ClassType = "C", Rating = 4.0, ImageSource = "img2" },
            // Restaurants
            new Item { Id = 3, Name = "Fine Dining", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 300, Score = CalculateScoreBehavior.CalculateScore("A", 5, 300), ClassType = "A", Rating = 4.7, ImageSource = "img3" },
            new Item { Id = 4, Name = "Local Cafe", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 100, Score = CalculateScoreBehavior.CalculateScore("C", 5, 100), ClassType = "C", Rating = 4.2, ImageSource = "img4" },
            // Entertainments
            new Item { Id = 5, Name = "Theme Park", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 400, Score = CalculateScoreBehavior.CalculateScore("B", 5, 400), ClassType = "B", Rating = 4.5, ImageSource = "img5" },
            new Item { Id = 6, Name = "Museum", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 50, Score = CalculateScoreBehavior.CalculateScore("C", 5, 50), ClassType = "C", Rating = 4.1, ImageSource = "img6" },
            // Tourism Areas
            new Item { Id = 7, Name = "Historical Site", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 200, Score = CalculateScoreBehavior.CalculateScore("A", 5, 200), ClassType = "A", Rating = 4.8, ImageSource = "img7" },
            new Item { Id = 8, Name = "Beach", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 50, Score = CalculateScoreBehavior.CalculateScore("C", 5, 50), ClassType = "C", Rating = 4.3, ImageSource = "img8" }
        };
    }

    private List<Item> CreateMixedItemsForBudget1500()
    {
        return new List<Item>
        {
            new Item { Id = 1, Name = "Hotel A", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 600, Score = CalculateScoreBehavior.CalculateScore("B", 5, 600), ClassType = "B", Rating = 4.5, ImageSource = "img1" },
            new Item { Id = 2, Name = "Hotel B", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 400, Score = CalculateScoreBehavior.CalculateScore("C", 5, 400), ClassType = "C", Rating = 4.0, ImageSource = "img2" },
            new Item { Id = 3, Name = "Restaurant A", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 200, Score = CalculateScoreBehavior.CalculateScore("A", 5, 200), ClassType = "A", Rating = 4.7, ImageSource = "img3" },
            new Item { Id = 4, Name = "Restaurant B", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 150, Score = CalculateScoreBehavior.CalculateScore("C", 5, 150), ClassType = "C", Rating = 4.2, ImageSource = "img4" },
            new Item { Id = 5, Name = "Theme Park", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 300, Score = CalculateScoreBehavior.CalculateScore("B", 5, 300), ClassType = "B", Rating = 4.6, ImageSource = "img5" },
            new Item { Id = 6, Name = "Cinema", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 100, Score = CalculateScoreBehavior.CalculateScore("C", 5, 100), ClassType = "C", Rating = 4.1, ImageSource = "img6" },
            new Item { Id = 7, Name = "Monument", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 100, Score = CalculateScoreBehavior.CalculateScore("A", 5, 100), ClassType = "A", Rating = 4.8, ImageSource = "img7" },
            new Item { Id = 8, Name = "Park", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 50, Score = CalculateScoreBehavior.CalculateScore("C", 5, 50), ClassType = "C", Rating = 4.3, ImageSource = "img8" },
            new Item { Id = 9, Name = "Restaurant C", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 250, Score = CalculateScoreBehavior.CalculateScore("B", 5, 250), ClassType = "B", Rating = 4.4, ImageSource = "img9" },
            new Item { Id = 10, Name = "Zoo", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 200, Score = CalculateScoreBehavior.CalculateScore("B", 5, 200), ClassType = "B", Rating = 4.5, ImageSource = "img10" }
        };
    }

    [Fact]
    public void Calculate_GivenBudget1000AndMixedItems_ReturnsMaxProfit18WithRestaurant1Accommodation1Entertainment1TourismArea1()
    {
        // Arrange
        int budget = 1000;
        int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
        var items = CreateStandardTestItems();
        float expectedProfit = 115 + 85 + 55 + 115; // Restaurant1 (A: 115), Accommodation1 (B: 85), Entertainment1 (C: 55), TourismArea1 (A: 115)

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(expectedProfit, dp[budget, maxR, maxA, maxE, maxT]);

        // Verify selected items
        Assert.True(decision[200, 1, 0, 0, 0, 0]);
        Assert.True(decision[700, 1, 1, 0, 0, 1]);
        Assert.True(decision[850, 1, 1, 1, 0, 2]);
        Assert.True(decision[950, 1, 1, 1, 1, 3]);
        Assert.Equal(1, itemIds[200, 1, 0, 0, 0, 0].GetValueOrDefault());
        Assert.Equal(2, itemIds[700, 1, 1, 0, 0, 1].GetValueOrDefault());
        Assert.Equal(3, itemIds[850, 1, 1, 1, 0, 2].GetValueOrDefault());
        Assert.Equal(4, itemIds[950, 1, 1, 1, 1, 3].GetValueOrDefault());


        _output.WriteLine($"Final Profit: {dp[budget, maxR, maxA, maxE, maxT]}");
        _output.WriteLine("Selected Items:");
        int currentW = budget, currentR = maxR, currentA = maxA, currentE = maxE, currentT = maxT;
        int totalCost = 0, totalScore = 0;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (decision[currentW, currentR, currentA, currentE, currentT, i])
            {
                var item = items[i];
                _output.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Cost: {item.AveragePricePerAdult}, Score: {item.Score}, ID: {itemIds[currentW, currentR, currentA, currentE, currentT, i]}");
                totalCost += (int)item.AveragePricePerAdult;
                totalScore += (int)item.Score;
                currentW -= (int)item.AveragePricePerAdult;
                switch (item.PlaceType)
                {
                    case ItemType.Restaurant: currentR--; break;
                    case ItemType.Accommodation: currentA--; break;
                    case ItemType.Entertainment: currentE--; break;
                    case ItemType.TourismArea: currentT--; break;
                }
            }
        }
        _output.WriteLine($"Total Cost: {totalCost}, Total Score: {totalScore}");
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenZeroBudget_ReturnsZeroProfitAndNoItemsSelected()
    {
        // Arrange
        int budget = 0;
        int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
        var items = CreateStandardTestItems();

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(0f, dp[budget, maxR, maxA, maxE, maxT]);
        for (int r = 0; r <= maxR; r++)
        for (int a = 0; a <= maxA; a++)
        for (int e = 0; e <= maxE; e++)
        for (int t = 0; t <= maxT; t++)
        for (int i = 0; i < items.Count; i++)
        {
            Assert.False(decision[budget, r, a, e, t, i], $"No items should be selected at state [{budget}, {r}, {a}, {e}, {t}]");
            Assert.Null(itemIds[budget, r, a, e, t, i]);
        }
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenEmptyItemList_ReturnsZeroProfitAndEmptyArrays()
    {
        // Arrange
        int budget = 1000;
        int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
        var items = new List<Item>();

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(0f, dp[budget, maxR, maxA, maxE, maxT]);
        for (int w = 0; w <= budget; w++)
        for (int r = 0; r <= maxR; r++)
        for (int a = 0; a <= maxA; a++)
        for (int e = 0; e <= maxE; e++)
        for (int t = 0; t <= maxT; t++)
        {
            Assert.Equal(0f, dp[w, r, a, e, t]);
        }
        Assert.Equal(0, decision.GetLength(5));
        Assert.Equal(0, itemIds.GetLength(5));
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenBudget50BelowMinCost100_ReturnsZeroProfitAndNoItemsSelected()
    {
        // Arrange
        int budget = 50;
        int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
        var items = CreateStandardTestItems();

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(0f, dp[budget, maxR, maxA, maxE, maxT]);
        for (int r = 0; r <= maxR; r++)
        for (int a = 0; a <= maxA; a++)
        for (int e = 0; e <= maxE; e++)
        for (int t = 0; t <= maxT; t++)
        for (int i = 0; i < items.Count; i++)
        {
            Assert.False(decision[budget, r, a, e, t, i], $"No items should be selected at state [{budget}, {r}, {a}, {e}, {t}]");
            Assert.Null(itemIds[budget, r, a, e, t, i]);
        }
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenOnlyRestaurantItemsAndBudget1000_ReturnsMaxProfit9WithBothRestaurants()
    {
        // Arrange
        int budget = 1000;
        int maxR = 2, maxA = 0, maxE = 0, maxT = 0;
        var items = new List<Item>
        {
            new Item { Id = 1, Name = "Restaurant1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 200, Score = CalculateScoreBehavior.CalculateScore("A", 5, 200), ClassType = "A", Rating = 4.5, ImageSource = "img1" },
            new Item { Id = 2, Name = "Restaurant2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 300, Score = CalculateScoreBehavior.CalculateScore("B", 5, 300), ClassType = "B", Rating = 4.3, ImageSource = "img2" }
        };
        float expectedProfit = 115 + 85; // Restaurant1 (A: 115) + Restaurant2 (B: 85)

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(expectedProfit, dp[budget, maxR, maxA, maxE, maxT]);
        Assert.True(decision[500, 1, 0, 0, 0, 0], "Restaurant1 should be selected at state [500, 1, 0, 0, 0]");
        Assert.True(decision[budget, 2, 0, 0, 0, 1], "Restaurant2 should be selected at state [1000, 2, 0, 0, 0]");
        Assert.Equal(1, itemIds[500, 1, 0, 0, 0, 0].GetValueOrDefault());
        Assert.Equal(2, itemIds[budget, 2, 0, 0, 0, 1].GetValueOrDefault());
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenAllZeroConstraints_ReturnsZeroProfitAndNoItemsSelected()
    {
        // Arrange
        int budget = 1000;
        int maxR = 0, maxA = 0, maxE = 0, maxT = 0;
        var items = CreateStandardTestItems();

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(0f, dp[budget, maxR, maxA, maxE, maxT]);
        for (int w = 0; w <= budget; w++)
        {
            Assert.Equal(0f, dp[w, maxR, maxA, maxE, maxT]);
        }
        for (int w = 0; w <= budget; w++)
        for (int i = 0; i < items.Count; i++)
        {
            Assert.False(decision[w, maxR, maxA, maxE, maxT, i], $"No items should be selected at state [{w}, {maxR}, {maxA}, {maxE}, {maxT}]");
            Assert.Null(itemIds[w, maxR, maxA, maxE, maxT, i]);
        }
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenNegativeBudget_ThrowsIndexOutOfRangeException()
    {
        // Arrange
        int budget = -1;
        int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
        var items = CreateStandardTestItems();

        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT));
    }

    [Fact]
    public void Calculate_Given100ItemsAndBudget5000_ReturnsNonZeroProfitWithValidSelections()
    {
        // Arrange
        int budget = 5000;
        int maxR = 4, maxA = 1, maxE = 2, maxT = 1;
        var items = CreateLargeTestItems(100);

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.True(dp[budget, maxR, maxA, maxE, maxT] > 0, "Profit should be non-zero with sufficient budget and items");
        bool hasSelections = false;
        for (int w = 0; w <= budget; w++)
        for (int r = 0; r <= maxR; r++)
        for (int a = 0; a <= maxA; a++)
        for (int e = 0; e <= maxE; e++)
        for (int t = 0; t <= maxT; t++)
        for (int i = 0; i < items.Count; i++)
        {
            if (decision[w, r, a, e, t, i])
            {
                hasSelections = true;
                Assert.NotNull(itemIds[w, r, a, e, t, i]);
            }
        }
        Assert.True(hasSelections, "At least some items should be selected within constraints");
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenBudget10000AndSingleItemConstraints_ReturnsMaxProfit345WithLuxuryHotelFineDiningThemeParkHistoricalSite()
    {
        // Arrange
        int budget = 10000;
        int maxR = 1, maxA = 1, maxE = 1, maxT = 1;
        var items = CreateItemsForHighBudget();
        float expectedProfit = 115 + 115 + 85 + 115; // Luxury Hotel (A: 115), Fine Dining (A: 115), Theme Park (B: 85), Historical Site (A: 115)

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(expectedProfit, dp[budget, maxR, maxA, maxE, maxT]);

        // Verify selected items
        Assert.True(decision[2000, 0, 1, 0, 0, 0], "Luxury Hotel should be selected");
        Assert.True(decision[2300, 1, 1, 0, 0, 2], "Fine Dining should be selected");
        Assert.True(decision[2700, 1, 1, 1, 0, 4], "Theme Park should be selected");
        Assert.True(decision[2900, 1, 1, 1, 1, 6], "Historical Site should be selected");
        Assert.Equal(1, itemIds[2000, 0, 1, 0, 0, 0].GetValueOrDefault());
        Assert.Equal(3, itemIds[2300, 1, 1, 0, 0, 2].GetValueOrDefault());
        Assert.Equal(5, itemIds[2700, 1, 1, 1, 0, 4].GetValueOrDefault());
        Assert.Equal(7, itemIds[2900, 1, 1, 1, 1, 6].GetValueOrDefault());

        // Debug: Print selected items
        _output.WriteLine($"Final Profit: {dp[budget, maxR, maxA, maxE, maxT]}");
        _output.WriteLine("Selected Items:");
        int currentW = budget, currentR = maxR, currentA = maxA, currentE = maxE, currentT = maxT;
        int totalCost = 0, totalScore = 0;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (decision[currentW, currentR, currentA, currentE, currentT, i])
            {
                var item = items[i];
                _output.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Cost: {item.AveragePricePerAdult}, Score: {item.Score}, ID: {itemIds[currentW, currentR, currentA, currentE, currentT, i]}");
                totalCost += (int)item.AveragePricePerAdult;
                totalScore += (int)item.Score;
                currentW -= (int)item.AveragePricePerAdult;
                switch (item.PlaceType)
                {
                    case ItemType.Restaurant: currentR--; break;
                    case ItemType.Accommodation: currentA--; break;
                    case ItemType.Entertainment: currentE--; break;
                    case ItemType.TourismArea: currentT--; break;
                }
            }
        }
        _output.WriteLine($"Total Cost: {totalCost}, Total Score: {totalScore}");
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }

    [Fact]
    public void Calculate_GivenBudget1500And10MixedItems_ReturnsMaxProfit510WithRestaurantAMonumentRestaurantCZoo()
    {
        // Arrange
        int budget = 1500;
        int maxR = 10, maxA = 10, maxE = 10, maxT = 10;
        var items = CreateMixedItemsForBudget1500();
        float expectedProfit = 115 + 115 + 85 + 85; // Restaurant A (A: 115), Monument (A: 115), Restaurant C (B: 85), Zoo (B: 85)

        // Act
        var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        Assert.Equal(expectedProfit, dp[budget, maxR, maxA, maxE, maxT]);

        // Verify selected items
        Assert.True(decision[200, 1, 0, 0, 0, 2], "Restaurant A should be selected");
        Assert.True(decision[300, 1, 0, 0, 1, 6], "Monument should be selected");
        Assert.True(decision[550, 2, 0, 0, 1, 8], "Restaurant C should be selected");
        Assert.True(decision[750, 2, 0, 1, 1, 9], "Zoo should be selected");
        Assert.Equal(3, itemIds[200, 1, 0, 0, 0, 2].GetValueOrDefault());
        Assert.Equal(7, itemIds[300, 1, 0, 0, 1, 6].GetValueOrDefault());
        Assert.Equal(9, itemIds[550, 2, 0, 0, 1, 8].GetValueOrDefault());
        Assert.Equal(10, itemIds[750, 2, 0, 1, 1, 9].GetValueOrDefault());

        // Debug: Print selected items
        _output.WriteLine($"Final Profit: {dp[budget, maxR, maxA, maxE, maxT]}");
        _output.WriteLine("Selected Items:");
        int currentW = budget, currentR = maxR, currentA = maxA, currentE = maxE, currentT = maxT;
        int totalCost = 0, totalScore = 0;
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (decision[currentW, currentR, currentA, currentE, currentT, i])
            {
                var item = items[i];
                _output.WriteLine($"Item: {item.Name}, Type: {item.PlaceType}, Cost: {item.AveragePricePerAdult}, Score: {item.Score}, ID: {itemIds[currentW, currentR, currentA, currentE, currentT, i]}");
                totalCost += (int)item.AveragePricePerAdult;
                totalScore += (int)item.Score;
                currentW -= (int)item.AveragePricePerAdult;
                switch (item.PlaceType)
                {
                    case ItemType.Restaurant: currentR--; break;
                    case ItemType.Accommodation: currentA--; break;
                    case ItemType.Entertainment: currentE--; break;
                    case ItemType.TourismArea: currentT--; break;
                }
            }
        }
        _output.WriteLine($"Total Cost: {totalCost}, Total Score: {totalScore}");
        _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
    }
}



/*
 *Fluent Assertion
 using System;
using System.Collections.Generic;
using FluentAssertions;
using TripMinder.Core.Behaviors.Knapsack;
using Xunit;
using Xunit.Abstractions;

namespace TripMinder.Core.Tests.Behaviors.Knapsack
{
    public class DynamicProgrammingCalculatorTests
    {
        private readonly IDynamicProgrammingCalculator _calculator;
        private readonly ITestOutputHelper _output;

        public DynamicProgrammingCalculatorTests(ITestOutputHelper output)
        {
            _calculator = new DynamicProgrammingCalculator();
            _output = output;
        }

        // Helper method to create a standard set of test items
        private List<Item> CreateStandardTestItems()
        {
            return new List<Item>
            {
                new Item { Id = 1, Name = "Restaurant1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 200, Score = 5, ClassType = "A", Rating = 4.5, ImageSource = "img1" },
                new Item { Id = 2, Name = "Accommodation1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 500, Score = 8, ClassType = "B", Rating = 4.8, ImageSource = "img2" },
                new Item { Id = 3, Name = "Entertainment1", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 150, Score = 3, ClassType = "C", Rating = 4.0, ImageSource = "img3" },
                new Item { Id = 4, Name = "TourismArea1", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 100, Score = 2, ClassType = "A", Rating = 4.2, ImageSource = "img4" },
                new Item { Id = 5, Name = "Restaurant2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 300, Score = 4, ClassType = "B", Rating = 4.3, ImageSource = "img5" }
            };
        }

        // Helper method to create a large set of test items
        private List<Item> CreateLargeTestItems(int count = 100)
        {
            var items = new List<Item>();
            for (int i = 0; i < count; i++)
            {
                items.Add(new Item
                {
                    Id = i + 1,
                    Name = $"Item{i + 1}",
                    PlaceType = (ItemType)(i % 4), // Cycles through Restaurant, Accommodation, Entertainment, TourismArea
                    AveragePricePerAdult = 100 + (i % 10) * 50, // Costs: 100, 150, 200, ..., 550
                    Score = 1 + (i % 5), // Scores: 1, 2, 3, 4, 5
                    ClassType = "A",
                    Rating = 4.0 + (i % 5) * 0.1,
                    ImageSource = $"img{i + 1}"
                });
            }
            return items;
        }

        [Fact]
        public void Calculate_GivenBudget1000AndMixedItems_ReturnsMaxProfit13WithRestaurant1AndAccommodation1()
        {
            // Arrange
            int budget = 1000;
            int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
            var items = CreateStandardTestItems();
            float expectedProfit = 13; // Restaurant1 (5) + Accommodation1 (8)

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().Be(expectedProfit, $"Expected profit {expectedProfit} at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            decision[700, 1, 0, 0, 0, 0].Should().BeTrue("Restaurant1 should be selected at state [700, 1, 0, 0, 0]");
            decision[budget, 1, 1, 0, 0, 1].Should().BeTrue("Accommodation1 should be selected at state [1000, 1, 1, 0, 0]");
            itemIds[700, 1, 0, 0, 0, 0].Should().Be(1, "Restaurant1 ID should be recorded at state [700, 1, 0, 0, 0]");
            itemIds[budget, 1, 1, 0, 0, 1].Should().Be(2, "Accommodation1 ID should be recorded at state [1000, 1, 1, 0, 0]");
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }

        [Fact]
        public void Calculate_GivenZeroBudget_ReturnsZeroProfitAndNoItemsSelected()
        {
            // Arrange
            int budget = 0;
            int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
            var items = CreateStandardTestItems();

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().Be(0, $"Profit should be 0 at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            for (int r = 0; r <= maxR; r++)
            for (int a = 0; a <= maxA; a++)
            for (int e = 0; e <= maxE; e++)
            for (int t = 0; t <= maxT; t++)
            for (int i = 0; i < items.Count; i++)
            {
                decision[budget, r, a, e, t, i].Should().BeFalse($"No items should be selected at state [{budget}, {r}, {a}, {e}, {t}]");
                itemIds[budget, r, a, e, t, i].Should().BeNull($"Item IDs should be null at state [{budget}, {r}, {a}, {e}, {t}]");
            }
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }

        [Fact]
        public void Calculate_GivenEmptyItemList_ReturnsZeroProfitAndEmptyArrays()
        {
            // Arrange
            int budget = 1000;
            int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
            var items = new List<Item>();

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().Be(0, $"Profit should be 0 at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            for (int w = 0; w <= budget; w++)
            for (int r = 0; r <= maxR; r++)
            for (int a = 0; a <= maxA; a++)
            for (int e = 0; e <= maxE; e++)
            for (int t = 0; t <= maxT; t++)
            {
                dp[w, r, a, e, t].Should().Be(0, $"DP table should be zero at state [{w}, {r}, {a}, {e}, {t}]");
            }
            decision.GetLength(5).Should().Be(0, "Decision array should have zero item dimension");
            itemIds.GetLength(5).Should().Be(0, "ItemIds array should have zero item dimension");
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }

        [Fact]
        public void Calculate_GivenBudget50BelowMinCost100_ReturnsZeroProfitAndNoItemsSelected()
        {
            // Arrange
            int budget = 50;
            int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
            var items = CreateStandardTestItems(); // Min cost = 100

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().Be(0, $"Profit should be 0 at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            for (int r = 0; r <= maxR; r++)
            for (int a = 0; a <= maxA; a++)
            for (int e = 0; e <= maxE; e++)
            for (int t = 0; t <= maxT; t++)
            for (int i = 0; i < items.Count; i++)
            {
                decision[budget, r, a, e, t, i].Should().BeFalse($"No items should be selected at state [{budget}, {r}, {a}, {e}, {t}]");
                itemIds[budget, r, a, e, t, i].Should().BeNull($"Item IDs should be null at state [{budget}, {r}, {a}, {e}, {t}]");
            }
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }

        [Fact]
        public void Calculate_GivenOnlyRestaurantItemsAndBudget1000_ReturnsMaxProfit9WithBothRestaurants()
        {
            // Arrange
            int budget = 1000;
            int maxR = 2, maxA = 0, maxE = 0, maxT = 0;
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Restaurant1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 200, Score = 5, ClassType = "A", Rating = 4.5, ImageSource = "img1" },
                new Item { Id = 2, Name = "Restaurant2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 300, Score = 4, ClassType = "B", Rating = 4.3, ImageSource = "img2" }
            };
            float expectedProfit = 9; // Restaurant1 (5) + Restaurant2 (4)

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().Be(expectedProfit, $"Expected profit {expectedProfit} at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            decision[500, 1, 0, 0, 0, 0].Should().BeTrue("Restaurant1 should be selected at state [500, 1, 0, 0, 0]");
            decision[budget, 2, 0, 0, 0, 1].Should().BeTrue("Restaurant2 should be selected at state [1000, 2, 0, 0, 0]");
            itemIds[500, 1, 0, 0, 0, 0].Should().Be(1, "Restaurant1 ID should be recorded at state [500, 1, 0, 0, 0]");
            itemIds[budget, 2, 0, 0, 0, 1].Should().Be(2, "Restaurant2 ID should be recorded at state [1000, 2, 0, 0, 0]");
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }

        [Fact]
        public void Calculate_GivenAllZeroConstraints_ReturnsZeroProfitAndNoItemsSelected()
        {
            // Arrange
            int budget = 1000;
            int maxR = 0, maxA = 0, maxE = 0, maxT = 0;
            var items = CreateStandardTestItems();

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().Be(0, $"Profit should be 0 at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            for (int w = 0; w <= budget; w++)
            {
                dp[w, maxR, maxA, maxE, maxT].Should().Be(0, $"DP table should be zero at state [{w}, {maxR}, {maxA}, {maxE}, {maxT}]");
            }
            for (int w = 0; w <= budget; w++)
            for (int i = 0; i < items.Count; i++)
            {
                decision[w, maxR, maxA, maxE, maxT, i].Should().BeFalse($"No items should be selected at state [{w}, {maxR}, {maxA}, {maxE}, {maxT}]");
                itemIds[w, maxR, maxA, maxE, maxT, i].Should().BeNull($"Item IDs should be null at state [{w}, {maxR}, {maxA}, {maxE}, {maxT}]");
            }
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }

        [Fact]
        public void Calculate_GivenNegativeBudget_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            int budget = -1;
            int maxR = 2, maxA = 1, maxE = 2, maxT = 1;
            var items = CreateStandardTestItems();

            // Act & Assert
            Action act = () => _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
            act.Should().Throw<IndexOutOfRangeException>("Negative budget should cause invalid array initialization");
        }

        [Fact]
        public void Calculate_Given100ItemsAndBudget5000_ReturnsNonZeroProfitWithValidSelections()
        {
            // Arrange
            int budget = 5000;
            int maxR = 4, maxA = 1, maxE = 2, maxT = 1;
            var items = CreateLargeTestItems(100);

            // Act
            var (dp, decision, itemIds) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

            // Assert
            dp[budget, maxR, maxA, maxE, maxT].Should().BeGreaterThan(0, $"Profit should be non-zero at state [{budget}, {maxR}, {maxA}, {maxE}, {maxT}]");
            bool hasSelections = false;
            for (int w = 0; w <= budget; w++)
            for (int r = 0; r <= maxR; r++)
            for (int a = 0; a <= maxA; a++)
            for (int e = 0; e <= maxE; e++)
            for (int t = 0; t <= maxT; t++)
            for (int i = 0; i < items.Count; i++)
            {
                if (decision[w, r, a, e, t, i])
                {
                    hasSelections = true;
                    itemIds[w, r, a, e, t, i].Should().NotBeNull($"Selected item at state [{w}, {r}, {a}, {e}, {t}] should have a valid ID");
                }
            }
            hasSelections.Should().BeTrue("At least some items should be selected within constraints");
            _output.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        }
    }
}
 * 
 */

