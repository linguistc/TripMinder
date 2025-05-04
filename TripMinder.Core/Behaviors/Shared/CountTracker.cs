namespace TripMinder.Core.Behaviors.Shared;

/// <summary>
/// Tracks how many items have been selected per type and ensures limits.
/// </summary>
public class CountTracker
{
    private readonly Dictionary<ItemType, int> _counts = new();

    public CountTracker()
    {
        foreach (ItemType t in Enum.GetValues(typeof(ItemType)))
            _counts[t] = 0;
    }

    public void Increment(ItemType type) => _counts[type]++;

    public int GetCount(ItemType type) => _counts[type];

    public bool Exceeded(ItemType type, IKnapsackConstraints constraints)
    {
        int max = type switch
        {
            ItemType.Accommodation => constraints.MaxAccommodations,
            ItemType.Restaurant => constraints.MaxRestaurants,
            ItemType.Entertainment => constraints.MaxEntertainments,
            ItemType.TourismArea => constraints.MaxTourismAreas,
            _ => 0
        };
        return _counts[type] >= max;
    }
}