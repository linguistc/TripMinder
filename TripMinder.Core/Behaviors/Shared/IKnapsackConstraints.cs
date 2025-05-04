namespace TripMinder.Core.Behaviors.Shared;

public interface IKnapsackConstraints
{
    int MaxRestaurants { get; }
    int MaxAccommodations { get; }
    int MaxEntertainments { get; }
    int MaxTourismAreas { get; }
}