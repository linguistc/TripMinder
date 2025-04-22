namespace TripMinder.Core.Behaviors;

public static class CalculateScoreBehavior
{
    public static float CalculateScore(string classType, int priority, double averagePricePerAdult)
    {
        float classWeight = classType == "A" ? 4 : classType == "B" ? 3 : classType == "C" ? 2 : 1;
        // float priceFactor = (float)(averagePricePerAdult / 1000); // التكلفة بتساهم في السكور
        return classWeight * priority;
    }
}