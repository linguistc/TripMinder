namespace TripMinder.Core.Behaviors.Shared;

/// <summary>
/// Result of priority calculation and helper methods for selection and count tracking.
/// </summary>
public class PriorityResult
{
    public int Accommodation { get; set; }
    public int Food { get; set; }
    public int Entertainment { get; set; }
    public int Tourism { get; set; }

    public PriorityResult(int accommodation, int food, int entertainment, int tourism)
    {
        Accommodation = accommodation;
        Food = food;
        Entertainment = entertainment;
        Tourism = tourism;
    }

    /// <summary>
    /// Returns the set of ItemType selected by user (priority > 0)
    /// </summary>
    public IReadOnlyCollection<ItemType> GetSelectedTypes()
    {
        var types = new List<ItemType>();
        if (Accommodation > 0) types.Add(ItemType.Accommodation);
        if (Food > 0) types.Add(ItemType.Restaurant);
        if (Entertainment > 0) types.Add(ItemType.Entertainment);
        if (Tourism > 0) types.Add(ItemType.TourismArea);
        return types;
    }

    /// <summary>
    /// Initializes a tracker for counts per type.
    /// </summary>
    public CountTracker InitializeCounts()
    {
        return new CountTracker();
    }
}