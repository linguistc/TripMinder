namespace TripMinder.Core.Behaviors.Shared;

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



// HELPER CLASSES
public record TripPlanRequest(
    int GovernorateId,
    int? ZoneId, 
    double BudgetPerAdult, 
    int NumberOfTravelers, 
    Queue<string> Interests, 
    int MaxRestaurants, 
    int MaxAccommodations, 
    int MaxEntertainments, 
    int MaxTourismAreas);
public record TripPlanResponse
{
    public ItemResponse Accommodation { get; init; }
    public List<ItemResponse> Restaurants { get; init; }
    public List<ItemResponse> Entertainments { get; init; }
    public List<ItemResponse> TourismAreas { get; init; }
}

public record ItemResponse(int Id, string Name, string ClassType, double AveragePricePerAdult, float Score, ItemType PlaceType, double Rating, string ImageSource);

public enum ItemType { Accommodation, Restaurant, Entertainment, TourismArea }

public class Item
{
    public int Id { get; set; }
    public string GlobalId { get; set; }
    public string Name { get; set; }
    public string ClassType { get; set; }
    public double AveragePricePerAdult { get; set; }
    public float Score { get; set; }
    public ItemType PlaceType { get; set; }
    public double Rating { get; set; }
    public string ImageSource { get; set; }
    public ItemResponse ToResponse() => new ItemResponse(Id, Name, ClassType, AveragePricePerAdult, Score, PlaceType, Rating, ImageSource);
}