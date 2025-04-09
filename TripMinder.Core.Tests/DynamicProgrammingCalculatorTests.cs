using TripMinder.Core.Behaviors.Knapsack;

namespace TripMinder.Core.Tests;

public class DynamicProgrammingCalculatorTests
{
    private readonly DynamicProgrammingCalculator _calculator = new DynamicProgrammingCalculator();
    private readonly int _budget = 1500;
    private readonly int _maxR = 2, _maxA = 1, _maxE = 2, _maxT = 1;

    private List<Item> GetTestItems()
    {
        return new List<Item>
        {
            new Item { Id = 1, Name = "A1", AveragePricePerAdult = 800, Score = 4, PlaceType = ItemType.Accommodation },
            new Item { Id = 2, Name = "A2", AveragePricePerAdult = 1000, Score = 5, PlaceType = ItemType.Accommodation },
            new Item { Id = 3, Name = "R1", AveragePricePerAdult = 200, Score = 2, PlaceType = ItemType.Restaurant },
            new Item { Id = 4, Name = "R2", AveragePricePerAdult = 300, Score = 3, PlaceType = ItemType.Restaurant },
            new Item { Id = 5, Name = "R3", AveragePricePerAdult = 100, Score = 1, PlaceType = ItemType.Restaurant },
            new Item { Id = 6, Name = "E1", AveragePricePerAdult = 400, Score = 3, PlaceType = ItemType.Entertainment },
            new Item { Id = 7, Name = "E2", AveragePricePerAdult = 150, Score = 2, PlaceType = ItemType.Entertainment },
            new Item { Id = 8, Name = "E3", AveragePricePerAdult = 500, Score = 4, PlaceType = ItemType.Entertainment },
            new Item { Id = 9, Name = "T1", AveragePricePerAdult = 250, Score = 3, PlaceType = ItemType.TourismArea },
            new Item { Id = 10, Name = "T2", AveragePricePerAdult = 600, Score = 4, PlaceType = ItemType.TourismArea }
        };
    }

    [Fact]
    public void Calculate_WithDiverseItems_ReturnsValidMaxProfit()
    {
        // Arrange
        var items = GetTestItems();

        // Act
        var (dp, _) = _calculator.Calculate(_budget, items, _maxR, _maxA, _maxE, _maxT);
        float maxProfit = dp[_budget, _maxR, _maxA, _maxE, _maxT];

        // Assert
        Assert.True(maxProfit > 0, "Maximum profit should be greater than 0 with valid items and budget.");
        float maxPossibleScore = items.Sum(i => i.Score); // 31
        Assert.True(maxProfit <= maxPossibleScore, "Maximum profit should not exceed total available score.");
        Assert.True(maxProfit >= 10, "Maximum profit should be reasonable (e.g., >= 10) given the item scores.");
    }

    [Fact]
    public void Calculate_WithDiverseItems_RespectsBudgetAndConstraints()
    {
        // Arrange
        var items = GetTestItems();

        // Act
        var (dp, decision) = _calculator.Calculate(_budget, items, _maxR, _maxA, _maxE, _maxT);
        var selectedItems = BacktrackItems(_budget, _maxR, _maxA, _maxE, _maxT, items, decision);

        // Assert
        int totalCost = selectedItems.Sum(i => (int)i.AveragePricePerAdult);
        Assert.True(totalCost <= _budget, $"Total cost ({totalCost}) should not exceed budget ({_budget}).");

        int rCount = selectedItems.Count(i => i.PlaceType == ItemType.Restaurant);
        int aCount = selectedItems.Count(i => i.PlaceType == ItemType.Accommodation);
        int eCount = selectedItems.Count(i => i.PlaceType == ItemType.Entertainment);
        int tCount = selectedItems.Count(i => i.PlaceType == ItemType.TourismArea);
        Assert.True(rCount <= _maxR, $"Selected Restaurants ({rCount}) should not exceed {_maxR}.");
        Assert.True(aCount <= _maxA, $"Selected Accommodations ({aCount}) should not exceed {_maxA}.");
        Assert.True(eCount <= _maxE, $"Selected Entertainments ({eCount}) should not exceed {_maxE}.");
        Assert.True(tCount <= _maxT, $"Selected Tourism Areas ({tCount}) should not exceed {_maxT}.");
    }

    [Fact]
    public void Calculate_WithInsufficientBudget_ReturnsZeroProfit()
    {
        // Arrange
        var items = GetTestItems();
        int lowBudget = 50;

        // Act
        var (dp, _) = _calculator.Calculate(lowBudget, items, _maxR, _maxA, _maxE, _maxT);

        // Assert
        Assert.True(dp[lowBudget, _maxR, _maxA, _maxE, _maxT] == 0f, "Profit should be 0 when budget is too low.");
    }

    [Fact]
    public void Calculate_WithZeroCostItem_IncludesItemInProfit()
    {
        // Arrange
        var items = GetTestItems();
        items.Add(new Item { Id = 11, Name = "R4", AveragePricePerAdult = 0, Score = 1, PlaceType = ItemType.Restaurant });

        // Act
        var (dp, _) = _calculator.Calculate(_budget, items, _maxR, _maxA, _maxE, _maxT);
        float profitWithZero = dp[_budget, _maxR, _maxA, _maxE, _maxT];
        var (dpWithoutZero, _) = _calculator.Calculate(_budget, GetTestItems(), _maxR, _maxA, _maxE, _maxT);
        float profitWithoutZero = dpWithoutZero[_budget, _maxR, _maxA, _maxE, _maxT];

        // Assert
        Assert.True(profitWithZero >= profitWithoutZero, "Profit should include zero-cost item if it fits constraints.");
    }
    
    
    [Fact]
    public void Calculate_WithDiverseItemCostsAndScores_ReturnsOptimalProfitAndDecision()
    {
        // Arrange
        var calculator = new DynamicProgrammingCalculator();
        int budget = 1500;
        int maxR = 2, maxA = 1, maxE = 2, maxT = 1;

        var items = new List<Item>
        {
            // Accommodations
            new Item { Id = 1, Name = "A1", AveragePricePerAdult = 800, Score = 4, PlaceType = ItemType.Accommodation },
            new Item
            {
                Id = 2, Name = "A2", AveragePricePerAdult = 1000, Score = 5, PlaceType = ItemType.Accommodation
            },
            // Restaurants
            new Item { Id = 3, Name = "R1", AveragePricePerAdult = 200, Score = 2, PlaceType = ItemType.Restaurant },
            new Item { Id = 4, Name = "R2", AveragePricePerAdult = 300, Score = 3, PlaceType = ItemType.Restaurant },
            new Item { Id = 5, Name = "R3", AveragePricePerAdult = 100, Score = 1, PlaceType = ItemType.Restaurant },
            // Entertainments
            new Item { Id = 6, Name = "E1", AveragePricePerAdult = 400, Score = 3, PlaceType = ItemType.Entertainment },
            new Item { Id = 7, Name = "E2", AveragePricePerAdult = 150, Score = 2, PlaceType = ItemType.Entertainment },
            new Item { Id = 8, Name = "E3", AveragePricePerAdult = 500, Score = 4, PlaceType = ItemType.Entertainment },
            // Tourism Areas
            new Item { Id = 9, Name = "T1", AveragePricePerAdult = 250, Score = 3, PlaceType = ItemType.TourismArea },
            new Item { Id = 10, Name = "T2", AveragePricePerAdult = 600, Score = 4, PlaceType = ItemType.TourismArea }
        };

        // Act
        var (dp, decision) = calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);

        // Assert
        // 1. Check if DP table reflects a valid maximum profit
        float maxProfit = dp[budget, maxR, maxA, maxE, maxT];
        Assert.True(maxProfit > 0, "Maximum profit should be greater than 0 with valid items and budget.");

        // 2. Validate profit is achievable and reasonable (e.g., between min and max possible scores)
        float maxPossibleScore = items.Sum(i => i.Score); // 31 in this case
        Assert.True(maxProfit <= maxPossibleScore, "Maximum profit should not exceed total available score.");
        Assert.True(maxProfit >= 10, "Maximum profit should be reasonable (e.g., >= 10) given the item scores.");

        // 3. Backtrack to verify selected items respect budget and constraints
        var selectedItems = BacktrackItems(budget, maxR, maxA, maxE, maxT, items, decision);
        int totalCost = selectedItems.Sum(i => (int)i.AveragePricePerAdult);
        Assert.True(totalCost <= budget, $"Total cost ({totalCost}) should not exceed budget ({budget}).");

        int rCount = selectedItems.Count(i => i.PlaceType == ItemType.Restaurant);
        int aCount = selectedItems.Count(i => i.PlaceType == ItemType.Accommodation);
        int eCount = selectedItems.Count(i => i.PlaceType == ItemType.Entertainment);
        int tCount = selectedItems.Count(i => i.PlaceType == ItemType.TourismArea);
        Assert.True(rCount <= maxR, $"Selected Restaurants ({rCount}) should not exceed {maxR}.");
        Assert.True(aCount <= maxA, $"Selected Accommodations ({aCount}) should not exceed {maxA}.");
        Assert.True(eCount <= maxE, $"Selected Entertainments ({eCount}) should not exceed {maxE}.");
        Assert.True(tCount <= maxT, $"Selected Tourism Areas ({tCount}) should not exceed {maxT}.");

        // 4. Edge Case: Check with insufficient budget (e.g., 50)
        var (dpLowBudget, decisionLowBudget) = calculator.Calculate(50, items, maxR, maxA, maxE, maxT);
        Assert.Equal(0f, dpLowBudget[50, maxR, maxA, maxE, maxT]);
        
        Assert.True(dpLowBudget[50, maxR, maxA, maxE, maxT] == 0f, "Profit should be 0 when budget is too low.");
        // 5. Edge Case: Item with zero cost
        items.Add(new Item
            { Id = 11, Name = "R4", AveragePricePerAdult = 0, Score = 1, PlaceType = ItemType.Restaurant });
        var (dpWithZero, decisionWithZero) = calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
        float profitWithZero = dpWithZero[budget, maxR, maxA, maxE, maxT];
        Assert.True(profitWithZero >= maxProfit, "Profit should include zero-cost item if it fits constraints.");
    }

    // Helper method to backtrack selected items from decision array
    private List<Item> BacktrackItems(int budget, int maxR, int maxA, int maxE, int maxT, List<Item> items, bool[,,,,,] decision)
    {
        var selectedItems = new List<Item>();
        int currentBudget = budget;
        int r = maxR, a = maxA, e = maxE, t = maxT;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (decision[currentBudget, r, a, e, t, i])
            {
                var item = items[i];
                selectedItems.Add(item);
                currentBudget -= (int)item.AveragePricePerAdult;
                r -= item.PlaceType == ItemType.Restaurant ? 1 : 0;
                a -= item.PlaceType == ItemType.Accommodation ? 1 : 0;
                e -= item.PlaceType == ItemType.Entertainment ? 1 : 0;
                t -= item.PlaceType == ItemType.TourismArea ? 1 : 0;
            }
        }

        return selectedItems;
    }
}

