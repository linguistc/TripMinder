namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackDP : IKnapsackDP
{
    private readonly IDynamicProgrammingCalculator _calculator;
    private readonly IKnapsackConstraints _constraints;
    
    public KnapsackDP(IDynamicProgrammingCalculator calculator, IKnapsackConstraints constraints)
    {
        _calculator = calculator;
        _constraints = constraints;
    }
    
    public (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) CalculateDP(int budget, List<Item> items)
    {
        return _calculator.Calculate(budget, items, _constraints.MaxRestaurants, _constraints.MaxAccommodations, 
            _constraints.MaxEntertainments, _constraints.MaxTourismAreas);
    }
}