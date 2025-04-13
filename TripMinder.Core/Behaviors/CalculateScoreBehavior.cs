namespace TripMinder.Core.Behaviors;

public static class CalculateScoreBehavior
{
    public static float CalculateScore(string classType, int priority, double averagePricePerAdult)
    {
        float classWeight = classType == "A" ? 100 : classType == "B" ? 75 : classType == "C" ? 50 : 10;
        float priceFactor = (float)(averagePricePerAdult / 1000); // التكلفة بتساهم في السكور
        return classWeight * priority * (1 + priceFactor);
    }
}