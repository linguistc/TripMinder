namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackSolver : IKnapsackSolver
{
    private const int MaxRestaurants = 3;
    private const int MaxAccommodations = 1;
    private const int MaxEntertainments = 3;
    private const int MaxTourismAreas = 3;

    private readonly KnapsackDP _dpCalculator;
    private readonly KnapsackBacktracker _backtracker;

    public KnapsackSolver(KnapsackDP dpCalculator, KnapsackBacktracker backtracker)
    {
        this._dpCalculator = dpCalculator;
        this._backtracker = backtracker;
    }

    public (float maxProfit, List<Item> selectedItems) GetMaxProfit(int budget, List<Item> items)
    {
        var (dp, decision) = _dpCalculator.CalculateDP(budget, items);

        // إيجاد أفضل نتيجة في نهاية الجدول
        float maxProfit = 0;
        int finalR = 0, finalA = 0, finalE = 0, finalT = 0;
        for (int r = 0; r <= MaxRestaurants; r++)
        {
            for (int a = 0; a <= MaxAccommodations; a++)
            {
                for (int e = 0; e <= MaxEntertainments; e++)
                {
                    for (int t = 0; t <= MaxTourismAreas; t++)
                    {
                        if (dp[budget, r, a, e, t] > maxProfit)
                        {
                            maxProfit = dp[budget, r, a, e, t];
                            finalR = r;
                            finalA = a;
                            finalE = e;
                            finalT = t;
                        }
                    }
                }
            }
        }

        // استرجاع العناصر المختارة عبر backtracking
        List<Item> selectedItems = new List<Item>();
        int currentW = budget, currentR = finalR, currentA = finalA, currentE = finalE, currentT = finalT;

        for (int i = items.Count - 1; i >= 0 && (currentR > 0 || currentA > 0 || currentE > 0 || currentT > 0); i--)
        {
            if (decision[currentW, currentR, currentA, currentE, currentT, i])
            {
                var selectedItem = items[i];
                selectedItems.Add(selectedItem);

                currentW -= (int)selectedItem.AveragePricePerAdult;
                if (selectedItem.PlaceType == ItemType.Restaurant) currentR--;
                if (selectedItem.PlaceType == ItemType.Accommodation) currentA--;
                if (selectedItem.PlaceType == ItemType.Entertainment) currentE--;
                if (selectedItem.PlaceType == ItemType.TourismArea) currentT--;
            }
        }

        return (maxProfit, selectedItems);
    }

    public (float maxProfit, List<List<Item>> allSelectedItems) GetMaxProfitMultiple(int budget, List<Item> items)
    {
        var (dp, decision) = _dpCalculator.CalculateDP(budget, items);

        float maxProfit = 0;
        int finalR = 0, finalA = 0, finalE = 0, finalT = 0;
        for (int r = 0; r <= MaxRestaurants; r++)
        {
            for (int a = 0; a <= MaxAccommodations; a++)
            {
                for (int e = 0; e <= MaxEntertainments; e++)
                {
                    for (int t = 0; t <= MaxTourismAreas; t++)
                    {
                        if (dp[budget, r, a, e, t] > maxProfit)
                        {
                            maxProfit = dp[budget, r, a, e, t];
                            finalR = r;
                            finalA = a;
                            finalE = e;
                            finalT = t;
                        }
                    }
                }
            }
        }

        // استخدام الهيب بدلاً من قائمة ضخمة
        SolutionOptimizer optimizer = new SolutionOptimizer(10);
        _backtracker.BacktrackAllSolutions(budget, finalR, finalA, finalE, finalT, items.Count - 1, items, decision, new List<Item>(), optimizer);

        return (maxProfit, optimizer.GetTopSolutions());
    }
}