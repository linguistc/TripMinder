using TripMinder.Core.Behaviors.Shared;

namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackState
{
    public double TotalProfit { get; set; }
    public int RemainingBudget { get; set; }
    public Dictionary<ItemType, int> CategoryCounts { get; set; }
    public HashSet<ItemType> UsedCategories { get; set; }
    public List<DpItem> SelectedItems { get; set; }
    public int Phase { get; set; }
    public (int a, int f, int e, int t)? Priorities { get; set; }

    public KnapsackState()
    {
        TotalProfit = 0;
        RemainingBudget = 0;
        CategoryCounts = new Dictionary<ItemType, int>();
        UsedCategories = new HashSet<ItemType>();
        SelectedItems = new List<DpItem>();
        Phase = 0;
        Priorities = null;
    }

    public KnapsackState(KnapsackState other)
    {
        TotalProfit = other.TotalProfit;
        RemainingBudget = other.RemainingBudget;
        CategoryCounts = new Dictionary<ItemType, int>(other.CategoryCounts);
        UsedCategories = new HashSet<ItemType>(other.UsedCategories);
        SelectedItems = new List<DpItem>(other.SelectedItems);
        Phase = other.Phase;
        Priorities = other.Priorities;
    }

    public KnapsackState Clone()
    {
        return new KnapsackState(this);
    }
}