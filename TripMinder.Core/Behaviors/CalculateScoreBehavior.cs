namespace TripMinder.Core.Behaviors;

public static class CalculateScoreBehavior
{
    public static float CalculateScore(string classType, int priority, double averagePricePerAdult)
    {
        float classWeight = classType == "A" ? 23 : classType == "B" ? 17 : classType == "C" ? 11 : 3;
        // float priceFactor = (float)(averagePricePerAdult / 1000); // التكلفة بتساهم في السكور
        return classWeight * priority;
    }
}