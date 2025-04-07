namespace TripMinder.Core.Behaviors.Knapsack;

public class DynamicProgrammingCalculator : IDynamicProgrammingCalculator
{
    public (float[,,,,] dp, bool[,,,,,] decision) Calculate(int budget, List<Item> items, int maxR, int maxA, int maxE, int maxT)
    {
        float[,,,,] dp = new float[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1];
        bool[,,,,,] decision = new bool[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1, items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            for (int w = budget; w >= (int)item.AveragePricePerAdult; w--)
            for (int r = maxR; r >= 0; r--)
            for (int a = maxA; a >= 0; a--)
            for (int e = maxE; e >= 0; e--)
            for (int t = maxT; t >= 0; t--)
            {
                if ((item.PlaceType == ItemType.Restaurant && r == 0) ||
                    (item.PlaceType == ItemType.Accommodation && a == 0) ||
                    (item.PlaceType == ItemType.Entertainment && e == 0) ||
                    (item.PlaceType == ItemType.TourismArea && t == 0))
                    continue;

                int newR = r - (item.PlaceType == ItemType.Restaurant ? 1 : 0);
                int newA = a - (item.PlaceType == ItemType.Accommodation ? 1 : 0);
                int newE = e - (item.PlaceType == ItemType.Entertainment ? 1 : 0);
                int newT = t - (item.PlaceType == ItemType.TourismArea ? 1 : 0);

                float profitWithItem = dp[w - (int)item.AveragePricePerAdult, newR, newA, newE, newT] + item.Score;
                if (profitWithItem > dp[w, r, a, e, t])
                {
                    dp[w, r, a, e, t] = profitWithItem;
                    decision[w, r, a, e, t, i] = true;
                }
            }
        }

        return (dp, decision);
    }
}