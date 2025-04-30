namespace TripMinder.Core.Behaviors;
using System;

public static class CalculateScoreBehavior
{
    /// <summary>
    /// يحسب سكور العنصر بناءً على:
    /// 1. فئة العنصر (classType) بأوزان أسّية (AHP-like).
    /// 2. أولوية المستخدم (priority) بأوزان متناقصة أُسّياً.
    /// 3. معامل سعر موحّد normalization بناءً على متوسط البودجت اليومي لكل فرد.
    /// </summary>
    /// <param name="classType">فئة العنصر (A, B, C, D)</param>
    /// <param name="priority">أولوية المستخدم (1 = أعلى)</param>
    /// <param name="averagePricePerAdult">متوسط سعر الفرد للعنصر</param>
    /// <param name="dailyBudgetPerAdult">البودجت اليومي لكل فرد</param>
    /// <returns>score قيمة العنصر المحسوبة</returns>
    public static float CalculateScore(
        string classType,
        int priority,
        double averagePricePerAdult,
        double dailyBudgetPerAdult)
    {
        // 1. Class Weight: أوزان أُسّية لضمان تفوق فئة أعلى
        float classWeight = classType switch
        {
            "A" => 16f,
            "B" => 8f,
            "C" => 4f,
            "D" => 2f,
            _   => 1f  // فئات أخرى بوزن منخفض جداً
        };

        // 2. Priority Weight: تناقص أُسّي بنسبة 20% بين الدرجات
        const float priorityDecay = 0.8f;
        float priorityWeight = priority > 0
            ? (float)Math.Pow(priorityDecay, priority - 1)
            : (float)Math.Pow(priorityDecay, 4);

        // 3. Price Normalization: دالة تعتمد على البودجت اليومي كمعامل مقياس
        //    سعر منخفض يعطي قيمة أقرب إلى 1، سعر = dailyBudget يعطي 0.5، سعر أعلى يقلل القيمة
        float scale = (float)Math.Max(dailyBudgetPerAdult, 1.0);
        float priceFactor = 1f / (1f + (float)(averagePricePerAdult / scale));

        // النتيجة النهائية: حاصل ضرب المكونات الثلاثة
        return classWeight * priorityWeight * priceFactor;
    }
}