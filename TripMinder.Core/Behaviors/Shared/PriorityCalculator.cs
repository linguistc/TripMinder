namespace TripMinder.Core.Behaviors.Shared;

/// <summary>
/// Calculates priorities based on ordered interests queue.
/// </summary>
public static class PriorityCalculator
{
    public static PriorityResult Calculate(Queue<string> interests)
    {
        int acc = 0, food = 0, ent = 0, tour = 0;
        int bonus = interests?.Count ?? 0;
        if (interests != null)
        {
            foreach (var raw in interests)
            {
                var interest = raw?.Trim().ToLowerInvariant();
                switch (interest)
                {
                    case "accommodation": acc = bonus--; break;
                    case "restaurants":
                    case "food": food = bonus--; break;
                    case "entertainments":
                    case "entertainment": ent = bonus--; break;
                    case "tourismareas":
                    case "tourism": tour = bonus--; break;
                }
            }
        }
        // if none set, default to all
        if ((interests?.Any() ?? false) && acc == 0 && food == 0 && ent == 0 && tour == 0)
        {
            acc = food = ent = tour = 1;
        }
        return new PriorityResult(acc, food, ent, tour);
    }
}