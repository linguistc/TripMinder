namespace TripMinder.Core.Behaviors.Knapsack;

public class ProfitFinder : IProfitFinder
{
    private const int MaxRestaurants = 3;
    private const int MaxAccommodations = 1;
    private const int MaxEntertainments = 3;
    private const int MaxTourismAreas = 3;

    public (float maxProfit, int r, int a, int e, int t) FindMaxProfit(float[,,,,] dp, int budget)
    {
        float maxProfit = 0;
        int finalR = 0, finalA = 0, finalE = 0, finalT = 0;
        for (int r = 0; r <= MaxRestaurants; r++)
        for (int a = 0; a <= MaxAccommodations; a++)
        for (int e = 0; e <= MaxEntertainments; e++)
        for (int t = 0; t <= MaxTourismAreas; t++)
        {
            if (dp[budget, r, a, e, t] > maxProfit)
            {
                maxProfit = dp[budget, r, a, e, t];
                finalR = r; finalA = a; finalE = e; finalT = t;
            }
        }
        return (maxProfit, finalR, finalA, finalE, finalT);
    }
}