namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackDP : IKnapsackDP
{
    private readonly IDynamicProgrammingCalculator _calculator;
    private readonly IKnapsackConstraints _constraints;
    
    public KnapsackDP(IDynamicProgrammingCalculator calculator, IKnapsackConstraints constraints)
    {
        this._calculator = calculator;
        this._constraints = constraints;
    }
    
    public (float[,,,,] dp, bool[,,,,,] decision) CalculateDP(int budget, List<Item> items)
    {
        return this._calculator.Calculate(budget, items, this._constraints.MaxRestaurants, this._constraints.MaxAccommodations, 
            this._constraints.MaxEntertainments, this._constraints.MaxTourismAreas);
    }
}