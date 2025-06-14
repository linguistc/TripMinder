using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class DpItem
{
    public int Id { get; }
    public string GlobalId { get; }
    public string Name { get; }
    public ItemType PlaceType { get; }
    public int Weight { get; }       // التكلفة (السعر)
    public float Profit { get; }     // الربح = item.Score
    public float Score { get; }      // نحتفظ به للـ logging
    public double AveragePricePerAdult { get; }

    public Item Original { get; }
    
    public DpItem(Item original)
    {
        Original = original;
        Id = original.Id;
        GlobalId = original.GlobalId;
        Name = original.Name;
        PlaceType = original.PlaceType;
        AveragePricePerAdult = original.AveragePricePerAdult;
        Weight = (int)original.AveragePricePerAdult;
        Score = original.Score;
        Profit = original.Score;
    }
}
