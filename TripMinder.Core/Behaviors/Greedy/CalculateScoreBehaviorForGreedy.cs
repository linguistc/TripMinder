namespace TripMinder.Core.Behaviors.Greedy;

public class CalculateScoreBehaviorForGreedy
{
    public static float CalculateScore(
        string classType,
        int priority,
        double averagePricePerAdult,
        double dailyBudgetPerAdult)
    {
        // Class Weight
        float classWeight = classType switch
        {
            "A" => 4f,
            "B" => 3f,
            "C" => 2f,
            "D" => 1f,
            _ => 0.5f
        };

        float priorityWeight = priority * 2f;

        // Price Normalization
        float scale = (float)Math.Max(dailyBudgetPerAdult, 1.0);
        float priceFactor = (float)(scale / (averagePricePerAdult + scale));

        return (classWeight + priorityWeight) * priceFactor;
    }

    public static float CalculateRatio(
        float score,
        double averagePricePerAdult,
        int priority)
    {
        float priorityFactor = priority * 2f;
        return priorityFactor * (score / (float)Math.Max(averagePricePerAdult, 1.0));
    }
}



public class UserDefinedKnapsackConstraints
{
    public int MaxRestaurants { get; }
    public int MaxAccommodations { get; }
    public int MaxEntertainments { get; }
    public int MaxTourismAreas { get; }

    public UserDefinedKnapsackConstraints(int maxRestaurants, int maxAccommodations, int maxEntertainments, int maxTourismAreas)
    {
        if (maxRestaurants < 0 || maxAccommodations < 0 || maxEntertainments < 0 || maxTourismAreas < 0)
            throw new ArgumentException("Max values cannot be negative.");

        MaxRestaurants = maxRestaurants;
        MaxAccommodations = maxAccommodations;
        MaxEntertainments = maxEntertainments;
        MaxTourismAreas = maxTourismAreas;
    }
}