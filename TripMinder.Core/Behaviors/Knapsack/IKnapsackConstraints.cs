namespace TripMinder.Core.Behaviors.Knapsack;

public interface IKnapsackConstraints
{
    int MaxRestaurants { get; }
    int MaxAccommodations { get; }
    int MaxEntertainments { get; }
    int MaxTourismAreas { get; }
}