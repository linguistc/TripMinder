using TripMinder.Core.Behaviors.Knapsack;

namespace TripMinder.Core.Tests;

public class DynamicProgrammingCalculatorExcessBudgetTests
{
    private readonly DynamicProgrammingCalculator _calculator = new DynamicProgrammingCalculator();

        private List<Item> GetTestItems()
        {
            return new List<Item>
            {
                new Item { Id = 1, Name = "A1", AveragePricePerAdult = 500, Score = 4, PlaceType = ItemType.Accommodation },
                new Item { Id = 2, Name = "A2", AveragePricePerAdult = 600, Score = 5, PlaceType = ItemType.Accommodation },
                new Item { Id = 3, Name = "R1", AveragePricePerAdult = 150, Score = 2, PlaceType = ItemType.Restaurant },
                new Item { Id = 4, Name = "R2", AveragePricePerAdult = 200, Score = 3, PlaceType = ItemType.Restaurant },
                new Item { Id = 5, Name = "E1", AveragePricePerAdult = 300, Score = 3, PlaceType = ItemType.Entertainment },
                new Item { Id = 6, Name = "E2", AveragePricePerAdult = 350, Score = 4, PlaceType = ItemType.Entertainment },
                new Item { Id = 7, Name = "T1", AveragePricePerAdult = 100, Score = 2, PlaceType = ItemType.TourismArea },
                new Item { Id = 8, Name = "T2", AveragePricePerAdult = 250, Score = 3, PlaceType = ItemType.TourismArea }
            };
        }

        [Fact]
        public void Calculate_WithExcessBudget_ReturnsValidMaxProfit()
        {
            // Arrange
            int budget = 10000;
            int maxR = 1, maxA = 1, maxE = 1, maxT = 1;
            var items = GetTestItems();

            // Act
            var (dp, _) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
            float maxProfit = dp[budget, maxR, maxA, maxE, maxT];

            // Assert
            Assert.True(maxProfit > 0, "Maximum profit should be greater than 0 with valid items.");
            float maxPossibleScore = 15; // Highest scores: 5 (A2) + 3 (R2) + 4 (E2) + 3 (T2)
            Assert.True(maxProfit <= maxPossibleScore, "Maximum profit should not exceed sum of highest scores per type.");
            Assert.True(maxProfit >= 11, "Maximum profit should be reasonable (e.g., >= 11) given item scores.");
        }

        [Fact]
        public void Calculate_WithExcessBudget_RespectsTightConstraints()
        {
            // Arrange
            int budget = 10000;
            int maxR = 1, maxA = 1, maxE = 1, maxT = 1;
            var items = GetTestItems();

            // Act
            var (dp, decision) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
            var selectedItems = BacktrackItems(budget, maxR, maxA, maxE, maxT, items, decision);

            // Assert
            int totalCost = selectedItems.Sum(i => (int)i.AveragePricePerAdult);
            Assert.True(totalCost <= 1950, $"Total cost ({totalCost}) should not exceed total item costs (1950).");

            int rCount = selectedItems.Count(i => i.PlaceType == ItemType.Restaurant);
            int aCount = selectedItems.Count(i => i.PlaceType == ItemType.Accommodation);
            int eCount = selectedItems.Count(i => i.PlaceType == ItemType.Entertainment);
            int tCount = selectedItems.Count(i => i.PlaceType == ItemType.TourismArea);
            Assert.True(rCount == 1, "Exactly 1 Restaurant should be selected.");
            Assert.True(aCount == 1, "Exactly 1 Accommodation should be selected.");
            Assert.True(eCount == 1, "Exactly 1 Entertainment should be selected.");
            Assert.True(tCount == 1, "Exactly 1 Tourism Area should be selected.");
        }

        [Fact]
        public void Calculate_WithHigherExcessBudget_MaintainsSameProfit()
        {
            // Arrange
            int budget = 10000;
            int higherBudget = 20000;
            int maxR = 1, maxA = 1, maxE = 1, maxT = 1;
            var items = GetTestItems();

            // Act
            var (dpNormal, _) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
            var (dpHigher, _) = _calculator.Calculate(higherBudget, items, maxR, maxA, maxE, maxT);
            float profitNormal = dpNormal[budget, maxR, maxA, maxE, maxT];
            float profitHigher = dpHigher[higherBudget, maxR, maxA, maxE, maxT];

            // Assert
            Assert.True(profitNormal == profitHigher, "Profit should remain unchanged with higher budget due to tight constraints.");
        }

        [Fact]
        public void Calculate_WithMissingItemType_ComputesCorrectly()
        {
            // Arrange
            int budget = 10000;
            int maxR = 1, maxA = 1, maxE = 1, maxT = 1;
            var items = GetTestItems();
            items.RemoveAll(i => i.PlaceType == ItemType.Restaurant); // No Restaurants

            // Act
            var (dp, _) = _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
            float maxProfit = dp[budget, maxR, maxA, maxE, maxT];

            // Assert
            Assert.True(maxProfit > 0, "Profit should still be positive with remaining item types.");
            float maxPossibleScore = 12; // Highest scores without Restaurants: 5 (A2) + 4 (E2) + 3 (T2)
            Assert.True(maxProfit <= maxPossibleScore, "Profit should not exceed sum of highest scores of available types.");
        }

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