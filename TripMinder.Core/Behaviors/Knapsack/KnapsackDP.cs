namespace TripMinder.Core.Behaviors.Knapsack;

public class KnapsackDP : IKnapsackDP
{
    private readonly IDynamicProgrammingCalculator _calculator;

    public KnapsackDP(IDynamicProgrammingCalculator calculator)
    {
        _calculator = calculator;
    }

    public (float[,,,,] dp, bool[,,,,,] decision, int?[,,,,,] itemIds) Calculate(
        int budget,
        List<DpItem> items,
        int maxR,
        int maxA,
        int maxE,
        int maxT)
    {
        return _calculator.Calculate(budget, items, maxR, maxA, maxE, maxT);
    }
}