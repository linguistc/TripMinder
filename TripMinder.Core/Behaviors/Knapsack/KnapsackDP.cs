namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackDP
{
    // القيود الافتراضية لكل فئة
    private const int MaxRestaurants = 3;
    private const int MaxAccommodations = 1;
    private const int MaxEntertainments = 3;
    private const int MaxTourismAreas = 3;

    public (float[,,,,] dp, bool[,,,,,] decision) CalculateDP(int budget, List<Item> items)
    {
        float[,,,,] dp = new float[budget + 1, MaxRestaurants + 1, MaxAccommodations + 1, MaxEntertainments + 1, MaxTourismAreas + 1];
        bool[,,,,,] decision = new bool[budget + 1, MaxRestaurants + 1, MaxAccommodations + 1, MaxEntertainments + 1, MaxTourismAreas + 1, items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            // المرور بالمصفوفة بطريقة عكسية
            for (int w = budget; w >= (int)item.AveragePricePerAdult; w--)
            {
                for (int r = MaxRestaurants; r >= 0; r--)
                {
                    for (int a = MaxAccommodations; a >= 0; a--)
                    {
                        for (int e = MaxEntertainments; e >= 0; e--)
                        {
                            for (int t = MaxTourismAreas; t >= 0; t--)
                            {
                                // التحقق من إن نوع العنصر يسمح بإدخاله
                                if ((item.PlaceType == ItemType.Restaurant && r == 0) ||
                                    (item.PlaceType == ItemType.Accommodation && a == 0) ||
                                    (item.PlaceType == ItemType.Entertainment && e == 0) ||
                                    (item.PlaceType == ItemType.TourismArea && t == 0))
                                {
                                    continue;
                                }

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
                    }
                }
            }
        }

        return (dp, decision);
    }
}