namespace TripMinder.Core.Behaviors.Knapsack;

public class ProfitFinder : IProfitFinder
{
    public (float maxProfit, int usedBudget, int r, int a, int e, int t) FindMaxProfit(
        float[,,,,] dp, 
        int budget, 
        IKnapsackConstraints constraints)
    {
        float maxProfit = float.MinValue;
        int bestW = 0, bestR = 0, bestA = 0, bestE = 0, bestT = 0;

        for (int w = 0; w <= budget; w++)
        for (int r = 0; r <= constraints.MaxRestaurants; r++)
        for (int a = 0; a <= constraints.MaxAccommodations; a++)
        for (int e = 0; e <= constraints.MaxEntertainments; e++)
        for (int t = 0; t <= constraints.MaxTourismAreas; t++)
        {
            float profit = dp[w, r, a, e, t];
            // Only consider states with at least one restaurant if maxRestaurants > 0
            if (profit != float.MinValue && (constraints.MaxRestaurants == 0 || r > 0))
            {
                if (profit > maxProfit || maxProfit == float.MinValue)
                {
                    maxProfit = profit;
                    bestW = w;
                    bestR = r;
                    bestA = a;
                    bestE = e;
                    bestT = t;
                    Console.WriteLine($"Found better profit: Profit={profit}, Budget={w}, Restaurants={r}, Accommodations={a}, Entertainments={e}, TourismAreas={t}");
                }
            }
        }

        if (maxProfit == float.MinValue)
        {
            Console.WriteLine("No valid profit found, returning base case.");
            maxProfit = 0;
            bestW = 0;
            bestR = 0;
            bestA = 0;
            bestE = 0;
            bestT = 0;
        }

        Console.WriteLine($"Max Profit: {maxProfit}, Used Budget: {bestW}, Restaurants: {bestR}, Accommodations: {bestA}, Entertainments: {bestE}, TourismAreas: {bestT}");
        return (maxProfit, bestW, bestR, bestA, bestE, bestT);    }
}
