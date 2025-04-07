namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackDP : IKnapsackDP
{
    private const int MaxRestaurants = 3;
    private const int MaxAccommodations = 1;
    private const int MaxEntertainments = 3;
    private const int MaxTourismAreas = 3;

    private readonly IDynamicProgrammingCalculator _calculator;
    
    public KnapsackDP(IDynamicProgrammingCalculator calculator)
    {
        this._calculator = calculator;
    }
    
    public (float[,,,,] dp, bool[,,,,,] decision) CalculateDP(int budget, List<Item> items)
    {
        return this._calculator.Calculate(budget, items, MaxRestaurants, MaxAccommodations, MaxEntertainments, MaxTourismAreas);
    }
}