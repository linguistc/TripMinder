namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackDP : IKnapsackDP
{
    private readonly IDynamicProgrammingCalculator _calculator;
    public KnapsackDP(IDynamicProgrammingCalculator calculator)
    {
        _calculator = calculator;
    }
    
    public (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) 
        CalculateDP(int budget,
            List<Item> items,
            IKnapsackConstraints constraints)
    {
        return _calculator.Calculate(
            budget, items,
            constraints.MaxRestaurants,
            constraints.MaxAccommodations, 
            constraints.MaxEntertainments,
            constraints.MaxTourismAreas);
    }
}