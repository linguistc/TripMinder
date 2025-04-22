namespace TripMinder.Core.Behaviors;

public static class CalculateScoreBehavior
{
    public static float CalculateScore(string classType, int priority, double averagePricePerAdult)
    {
        float classWeight = classType switch
        {
            "A" => 4f,
            "B" => 3f,
            "C" => 2f,
            _ => 1f
        };
        float priorityWeight = priority > 0 ? priority : 0.5f; // Ensure non-zero priority has some weight
        return classWeight * priorityWeight;
    }
}
