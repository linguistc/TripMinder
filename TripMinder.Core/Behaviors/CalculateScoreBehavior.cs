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
            "D" => 1f,
            _ => 0.5f
        };
        // تقليل الفرق بين الأولويات
        float priorityWeight = priority > 0 ? (1.2f - (priority - 1) * 0.1f) : 0.5f; // 1.2, 1.1, 1.0, 0.9
        
        // Penalization للسعر العالي
        float pricePenalty = averagePricePerAdult > 2000 ? 0.8f : averagePricePerAdult > 1000 ? 0.9f : 1f;
        
        // مكافأة للسعر المنخفض
        float priceBonus = averagePricePerAdult < 100 ? 1.2f : 1f;
        
        return classWeight * priorityWeight * pricePenalty * priceBonus;
    }
}