namespace TripMinder.Core.Behaviors.Knapsack;

public class ProfitFinder : IProfitFinder
{
    private readonly IKnapsackConstraints _constraints;

    public ProfitFinder(IKnapsackConstraints constraints)
    {
        _constraints = constraints;
    }

    public (float maxProfit, int r, int a, int e, int t) FindMaxProfit(float[,,,,] dp, int budget)
    {
        var combinations = from r in Enumerable.Range(0, _constraints.MaxRestaurants + 1)
            from a in Enumerable.Range(0, _constraints.MaxAccommodations + 1)
            from e in Enumerable.Range(0, _constraints.MaxEntertainments + 1)
            from t in Enumerable.Range(0, _constraints.MaxTourismAreas + 1)
            select (r, a, e, t, profit: dp[budget, r, a, e, t]);

        var best = combinations.OrderByDescending(x => x.profit).First();
        return (best.profit, best.r, best.a, best.e, best.t);
    }
}

// Using LINQ but low performance bcz of Overhead
/*
public class ProfitFinder : IProfitFinder
{
    private readonly IKnapsackConstraints _constraints;

    public ProfitFinder(IKnapsackConstraints constraints)
    {
        _constraints = constraints;
    }

    public (float maxProfit, int r, int a, int e, int t) FindMaxProfit(float[,,,,] dp, int budget)
    {
        var combinations = from r in Enumerable.Range(0, _constraints.MaxRestaurants + 1)
                          from a in Enumerable.Range(0, _constraints.MaxAccommodations + 1)
                          from e in Enumerable.Range(0, _constraints.MaxEntertainments + 1)
                          from t in Enumerable.Range(0, _constraints.MaxTourismAreas + 1)
                          select (r, a, e, t, profit: dp[budget, r, a, e, t]);

        var best = combinations.OrderByDescending(x => x.profit).First();
        return (best.profit, best.r, best.a, best.e, best.t);
    }
} 
*/