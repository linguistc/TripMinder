namespace TripMinder.Core.Behaviors.Shared;
using System;

public static class CalculateScoreBehavior
{
    /// <summary>
    /// Calculates the score of an item based on:
    /// 1. Class type with exponential weights (AHP-like).
    /// 2. User priority with linear scaling to emphasize higher priorities.
    /// 3. Price normalization relative to daily budget to balance expensive items.
    /// </summary>
    /// <param name="classType">Item class (A, B, C, D)</param>
    /// <param name="priority">User priority (4 = highest, 1 = lowest)</param>
    /// <param name="averagePricePerAdult">Average price per adult for the item</param>
    /// <param name="dailyBudgetPerAdult">Daily budget per adult</param>
    /// <returns>Calculated item score</returns>
    public static float CalculateScore(
        string classType,
        int priority,
        double averagePricePerAdult,
        double dailyBudgetPerAdult)
    {
        // 1. Class Weight: Exponential weights for higher classes
        float classWeight = classType switch
        {
            "A" => 16f,
            "B" => 8f,
            "C" => 4f,
            "D" => 2f,
            _   => 1f
        };

        // 2. Priority Weight: Linear scaling to emphasize priority without harsh decay
        float priorityWeight = 1.0f + (priority - 1) * 2.0f; // e.g., 1.0, 3.0, 5.0, 7.0 for priorities 4,3,2,1

        // 3. Price Normalization: Logarithmic scaling to reduce penalty for high prices
        float scale = (float)Math.Max(dailyBudgetPerAdult, 1.0);
        float priceRatio = (float)(averagePricePerAdult / scale);
        float priceFactor = 1.0f / (1.0f + (float)Math.Log(1.0 + priceRatio)); // Logarithmic to soften high-price penalty

        // Final score: Product of components
        return classWeight * priorityWeight * priceFactor;
    }
}