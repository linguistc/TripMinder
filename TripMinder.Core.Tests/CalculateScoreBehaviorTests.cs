using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Tests.Behaviors.Knapsack;

public class CalculateScoreBehaviorTests
{

    [Fact]
    public void CalculateScoreBehaviorTests_PriceFactorUnity_ReturnsClassPriorityProduct()
    {
        // Arrange
        string classType = "C";
        int priority = 2;
        double averagePricePerAdult = 0.0;
        double dailyBudgetPerAdult = 100.0;

        // Act
        float score = CalculateScoreBehavior.CalculateScore(
            classType,
            priority,
            averagePricePerAdult,
            dailyBudgetPerAdult);

        // Assert
        Assert.Equal(12f, score);
    }

    [Theory]
    [InlineData("A", 4, 0.0, 100.0, 112f)]
    [InlineData("B", 3, 0.0, 100.0, 40f)]
    [InlineData("C", 2, 0.0, 100.0, 12f)]
    [InlineData("D", 1, 0.0, 100.0, 2f)]
    [InlineData("X", 1, 0.0, 100.0, 1f)]
    public void CalculateScoreBehavior_Theory_ClassPriorityProduct(
        string classType,
        int priority,
        double averagePricePerAdult,
        double dailyBudgetPerAdult,
        float expectedScore)
    {
        // Act
        float actualScore = CalculateScoreBehavior.CalculateScore(
            classType,
            priority,
            averagePricePerAdult,
            dailyBudgetPerAdult);

        // Assert
        Assert.Equal(expectedScore, actualScore);
    }

}