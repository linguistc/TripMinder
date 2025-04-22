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
            bool valid = profit != float.MinValue
                && (constraints.MaxRestaurants == 0 || r > 0)
                && (constraints.MaxAccommodations == 0 || a > 0)
                && (constraints.MaxEntertainments == 0 || e > 0)
                && (constraints.MaxTourismAreas == 0 || t > 0);

            if (valid && (profit > maxProfit || (profit == maxProfit && w < bestW)))
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
        return (maxProfit, bestW, bestR, bestA, bestE, bestT);
    }
}