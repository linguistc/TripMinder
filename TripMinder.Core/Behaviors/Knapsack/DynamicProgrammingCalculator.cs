namespace TripMinder.Core.Behaviors.Knapsack;

public class DynamicProgrammingCalculator : IDynamicProgrammingCalculator
{
    public (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(
        int budget, List<DpItem> items, int maxR, int maxA, int maxE, int maxT)
    {
        float[,,,,] dp = new float[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1];
        bool[,,,,,] decision = new bool[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1, items.Count];
        int?[,,,,,] itemIds = new int?[budget + 1, maxR + 1, maxA + 1, maxE + 1, maxT + 1, items.Count];

        // Log items for debugging
        Console.WriteLine($"Processing {items.Count} items: {string.Join(", ", items.Select(i => $"{i.Name} (GlobalId={i.GlobalId}, Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        // Initialize dp with float.MinValue
        // استخدام DpItem.Profit و DpItem.Weight:
        for (int w = budget; w >= 0; w--)
        for (int r = maxR; r >= 0; r--)
        for (int a = maxA; a >= 0; a--)
        for (int e = maxE; e >= 0; e--)
        for (int t = maxT; t >= 0; t--)
        {
            dp[w, r, a, e, t] = float.MinValue;
        }
        dp[0, 0, 0, 0, 0] = 0;

        // Run DP for all items
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            int cost = (int)item.Weight;
            bool isRestaurant = item.PlaceType == ItemType.Restaurant;
            bool isAccommodation = item.PlaceType == ItemType.Accommodation;
            bool isEntertainment = item.PlaceType == ItemType.Entertainment;
            bool isTourismArea = item.PlaceType == ItemType.TourismArea;

            // Skip items if their type has zero capacity
            if ((isRestaurant && maxR == 0) ||
                (isAccommodation && maxA == 0) ||
                (isEntertainment && maxE == 0) ||
                (isTourismArea && maxT == 0))
            {
                Console.WriteLine($"Skipping item: {item.Name}, GlobalId={item.GlobalId}, Type={item.PlaceType} (zero capacity)");
                continue;
            }
            
            for (int w = budget; w >= cost; w--)
            for (int r = maxR; r >= (isRestaurant ? 1 : 0); r--)
            for (int a = maxA; a >= (isAccommodation ? 1 : 0); a--)
            for (int e = maxE; e >= (isEntertainment ? 1 : 0); e--)
            for (int t = maxT; t >= (isTourismArea ? 1 : 0); t--)
            {
                // Option to include the item
                int newR = isRestaurant ? r - 1 : r;
                int newA = isAccommodation ? a - 1 : a;
                int newE = isEntertainment ? e - 1 : e;
                int newT = isTourismArea ? t - 1 : t;

                var prev = dp[w - cost, r - (isRestaurant ? 1 : 0), a - (isAccommodation ? 1 : 0), e - (isEntertainment ? 1 : 0), t - (isTourismArea ? 1 : 0)];

                if (prev != float.MinValue)
                {
                    float score = Math.Max(item.Score, 0.01f); // Ensure positive score
                    float with = prev + score;
                    if (with > dp[w, r, a, e, t])
                    {
                        dp[w, r, a, e, t] = with;
                        decision[w, r, a, e, t, i] = true;
                        itemIds[w, r, a, e, t, i] = item.Id;
                        Console.WriteLine($"DP Update: Item={item.Name}, GlobalId={item.GlobalId}, Type={item.PlaceType}, Budget={w}, Restaurants={r}, Score={score}, Profit={with}, Decision=True");
                    }
                    else
                    {
                        decision[w, r, a, e, t, i] = false;
                        Console.WriteLine($"DP Update: Item={item.Name}, GlobalId={item.GlobalId}, Type={item.PlaceType}, Budget={w}, Restaurants={r}, Score={score}, Profit={dp[w, r, a, e, t]}, Decision=False");
                    }
                }
            }
        }

        Console.WriteLine($"DP Complete: DP[{budget},{maxR},{maxA},{maxE},{maxT}]={dp[budget, maxR, maxA, maxE, maxT]}");
        return (dp, decision, itemIds);
    }
}