namespace TripMinder.Core.Behaviors.Knapsack;

public class UserDefinedKnapsackConstraints : IKnapsackConstraints
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