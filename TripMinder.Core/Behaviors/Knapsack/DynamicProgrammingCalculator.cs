namespace TripMinder.Core.Behaviors.Knapsack;

public class DynamicProgrammingCalculator : IDynamicProgrammingCalculator
{
    public (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(int budget, List<Item> items, int maxR, int maxA, int maxE, int maxT)
    {
        float[,,,,] dp = new float[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1];
        bool[,,,,,] decision = new bool[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1, items.Count];
        int?[,,,,,] itemIds = new int?[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1, items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            int cost = (int)item.AveragePricePerAdult;

            for (int w = budget; w >= cost; w--)
            for (int r = maxR; r >= 0; r--)
            for (int a = maxA; a >= 0; a--)
            for (int e = maxE; e >= 0; e--)
            for (int t = maxT; t >= 0; t--)
            {
                float profitWithoutItem = dp[w, r, a, e, t];
                int newR = r - (item.PlaceType == ItemType.Restaurant ? 1 : 0);
                int newA = a - (item.PlaceType == ItemType.Accommodation ? 1 : 0);
                int newE = e - (item.PlaceType == ItemType.Entertainment ? 1 : 0);
                int newT = t - (item.PlaceType == ItemType.TourismArea ? 1 : 0);

                if (newR >= 0 && newA >= 0 && newE >= 0 && newT >= 0)
                {
                    float profitWithItem = dp[w - cost, newR, newA, newE, newT] + item.Score;
                    if (profitWithItem > profitWithoutItem)
                    {
                        dp[w, r, a, e, t] = profitWithItem;
                        decision[w, r, a, e, t, i] = true;
                        itemIds[w, r, a, e, t, i] = item.Id;
                    }
                }
            }
        }

        Console.WriteLine($"DP[{budget}, {maxR}, {maxA}, {maxE}, {maxT}] = {dp[budget, maxR, maxA, maxE, maxT]}");
        return (dp, decision, itemIds);
    }
}