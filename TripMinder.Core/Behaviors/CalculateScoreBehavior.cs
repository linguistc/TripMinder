namespace TripMinder.Core.Behaviors;

public static class CalculateScoreBehavior
{
    public static float CalculateScore(string classType, int priority)
    {
        float classWeight = classType == "A" ? 4 : classType == "B" ? 3 : classType == "C" ? 2 : 1;
        return classWeight * priority;
    }
}