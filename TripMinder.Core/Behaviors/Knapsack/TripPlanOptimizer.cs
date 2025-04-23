using TripMinder.Core.Bases;

namespace TripMinder.Core.Behaviors.Knapsack;

public partial class TripPlanOptimizer
{
    private readonly IKnapsackSolver _solver;
    private readonly IItemFetcher _itemFetcher;

    public TripPlanOptimizer(IKnapsackSolver solver, IItemFetcher itemFetcher)
    {
        _solver = solver;
        _itemFetcher = itemFetcher;
    }

    public async Task<Respond<TripPlanResponse>> OptimizePlan(TripPlanRequest request)
    {
        // 1. Calculate priorities
        var priorities = CalculatePriorities(request.Interests);
        Console.WriteLine($"Calculated Priorities: Accommodation={priorities.accommodation}, Food={priorities.food}, Entertainment={priorities.entertainment}, Tourism={priorities.tourism}");

        // 2. Fetch and filter items
        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);
        var desiredTypes = new HashSet<ItemType>();
        if (priorities.accommodation > 0) desiredTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
        var filteredItems = allItems.Where(i => desiredTypes.Contains(i.PlaceType)).ToList();
        Console.WriteLine($"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}");

        // 3. Staged approach
        int remainingBudget = (int)request.BudgetPerAdult;
        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);
        var selectedItems = new List<Item>();
        var currentCounts = new Dictionary<ItemType, int>
        {
            { ItemType.Restaurant, 0 },
            { ItemType.Accommodation, 0 },
            { ItemType.Entertainment, 0 },
            { ItemType.TourismArea, 0 }
        };

        // Order interests by priority
        var orderedInterests = request.Interests.Select((interest, index) => (interest, priority: priorities.accommodation + priorities.food + priorities.entertainment + priorities.tourism - index))
            .OrderByDescending(x => x.priority)
            .Select(x => x.interest)
            .ToList();

        // Stage 1: Add one item per priority
        foreach (var interest in orderedInterests)
        {
            var itemType = GetItemType(interest);
            if (!desiredTypes.Contains(itemType) || currentCounts[itemType] >= GetMaxCount(itemType, constraints)) continue;

            var stageConstraints = new UserDefinedKnapsackConstraints(
                itemType == ItemType.Restaurant ? currentCounts[ItemType.Restaurant] + 1 : currentCounts[ItemType.Restaurant],
                itemType == ItemType.Accommodation ? currentCounts[ItemType.Accommodation] + 1 : currentCounts[ItemType.Accommodation],
                itemType == ItemType.Entertainment ? currentCounts[ItemType.Entertainment] + 1 : currentCounts[ItemType.Entertainment],
                itemType == ItemType.TourismArea ? currentCounts[ItemType.TourismArea] + 1 : currentCounts[ItemType.TourismArea]
            );

            var (maxProfit, stageItems) = _solver.GetMaxProfit(remainingBudget, filteredItems.Except(selectedItems).ToList(), stageConstraints, priorities, true); // requireExact = true
            if (stageItems.Any())
            {
                var newItem = stageItems.FirstOrDefault(i => i.PlaceType == itemType);
                if (newItem != null && remainingBudget >= (int)newItem.AveragePricePerAdult)
                {
                    selectedItems.Add(newItem);
                    remainingBudget -= (int)newItem.AveragePricePerAdult;
                    currentCounts[itemType]++;
                    Console.WriteLine($"Stage Item Selected: {newItem.Name}, Type={newItem.PlaceType}, Price={newItem.AveragePricePerAdult}, Remaining Budget={remainingBudget}");
                }
            }
        }

        // Stage 2: Add additional items if budget and constraints allow
        while (remainingBudget > 0 && currentCounts.Any(kv => kv.Value < GetMaxCount(kv.Key, constraints)))
        {
            bool added = false;
            foreach (var interest in orderedInterests)
            {
                var itemType = GetItemType(interest);
                if (!desiredTypes.Contains(itemType) || currentCounts[itemType] >= GetMaxCount(itemType, constraints)) continue;

                var stageConstraints = new UserDefinedKnapsackConstraints(
                    itemType == ItemType.Restaurant ? currentCounts[ItemType.Restaurant] + 1 : currentCounts[ItemType.Restaurant],
                    itemType == ItemType.Accommodation ? currentCounts[ItemType.Accommodation] + 1 : currentCounts[ItemType.Accommodation],
                    itemType == ItemType.Entertainment ? currentCounts[ItemType.Entertainment] + 1 : currentCounts[ItemType.Entertainment],
                    itemType == ItemType.TourismArea ? currentCounts[ItemType.TourismArea] + 1 : currentCounts[ItemType.TourismArea]
                );

                var (maxProfit, stageItems) = _solver.GetMaxProfit(remainingBudget, filteredItems.Except(selectedItems).ToList(), stageConstraints, priorities, true);
                if (stageItems.Any())
                {
                    var newItem = stageItems.FirstOrDefault(i => i.PlaceType == itemType);
                    if (newItem != null && remainingBudget >= (int)newItem.AveragePricePerAdult)
                    {
                        selectedItems.Add(newItem);
                        remainingBudget -= (int)newItem.AveragePricePerAdult;
                        currentCounts[itemType]++;
                        added = true;
                        Console.WriteLine($"Additional Item Selected: {newItem.Name}, Type={newItem.PlaceType}, Price={newItem.AveragePricePerAdult}, Remaining Budget={remainingBudget}");
                    }
                }
            }
            if (!added) break; // No more items can be added
        }

        // 4. Build response
        var tripPlanResponse = BuildTripPlanResponse(selectedItems, request);
        if (selectedItems.Any())
        {
            return new Respond<TripPlanResponse>
            {
                Succeeded = true,
                Message = "Trip plan optimized successfully",
                Data = tripPlanResponse,
                Meta = new { TotalItems = selectedItems.Count, TotalSolutions = 1 }
            };
        }

        return new Respond<TripPlanResponse>
        {
            Succeeded = false,
            Message = "No validpony valid trip plan found",
            Errors = new List<string> { "Unable to generate a solution within constraints" }
        };
    }

    private ItemType GetItemType(string interest)
    {
        return interest?.Trim().ToLowerInvariant() switch
        {
            "accommodation" => ItemType.Accommodation,
            "restaurants" or "food" => ItemType.Restaurant,
            "entertainments" or "entertainment" => ItemType.Entertainment,
            "tourismareas" or "tourism" => ItemType.TourismArea,
            _ => ItemType.Restaurant // Default
        };
    }

    private int GetMaxCount(ItemType itemType, IKnapsackConstraints constraints)
    {
        return itemType switch
        {
            ItemType.Restaurant => constraints.MaxRestaurants,
            ItemType.Accommodation => constraints.MaxAccommodations,
            ItemType.Entertainment => constraints.MaxEntertainments,
            ItemType.TourismArea => constraints.MaxTourismAreas,
            _ => 0
        };
    }

    private TripPlanResponse BuildTripPlanResponse(List<Item> selectedItems, TripPlanRequest request)
    {
        var response = new TripPlanResponse
        {
            Accommodation = selectedItems.FirstOrDefault(i => i.PlaceType == ItemType.Accommodation)?.ToResponse(),
            Restaurants = selectedItems.Where(i => i.PlaceType == ItemType.Restaurant)
                .Take(request.MaxRestaurants).Select(i => i.ToResponse()).ToList(),
            Entertainments = selectedItems.Where(i => i.PlaceType == ItemType.Entertainment)
                .Take(request.MaxEntertainments).Select(i => i.ToResponse()).ToList(),
            TourismAreas = selectedItems.Where(i => i.PlaceType == ItemType.TourismArea)
                .Take(request.MaxTourismAreas).Select(i => i.ToResponse()).ToList()
        };

        Console.WriteLine($"Built Response: Accommodation={(response.Accommodation?.Name ?? "None")}, Restaurants={response.Restaurants.Count}, Entertainments={response.Entertainments.Count}, TourismAreas={response.TourismAreas.Count}");
        return response;
    }

    private (int accommodation, int food, int entertainment, int tourism) CalculatePriorities(Queue<string> interests)
    {
        int accommodationPriority = 0, foodPriority = 0, entertainmentPriority = 0, tourismPriority = 0;
        int bonus = interests?.Count ?? 0;

        Console.WriteLine($"Calculating priorities for interests: {string.Join(", ", interests ?? new Queue<string>())}");

        if (interests != null)
        {
            foreach (var interest in interests)
            {
                var normalizedInterest = interest?.Trim().ToLowerInvariant();
                Console.WriteLine($"Processing interest: {normalizedInterest}");
                switch (normalizedInterest)
                {
                    case "accommodation":
                        accommodationPriority = bonus--;
                        break;
                    case "restaurants":
                    case "food":
                        foodPriority = bonus--;
                        break;
                    case "entertainments":
                    case "entertainment":
                        entertainmentPriority = bonus--;
                        break;
                    case "tourismareas":
                    case "tourism":
                        tourismPriority = bonus--;
                        break;
                    default:
                        Console.WriteLine($"Unrecognized interest: {normalizedInterest}");
                        break;
                }
            }
        }

        if ((interests?.Any() ?? false) && accommodationPriority == 0 && foodPriority == 0 && entertainmentPriority == 0 && tourismPriority == 0)
        {
            Console.WriteLine("No valid priorities set, defaulting to all priorities");
            accommodationPriority = foodPriority = entertainmentPriority = tourismPriority = 1;
        }

        Console.WriteLine($"Calculated Priorities: Accommodation={accommodationPriority}, Food={foodPriority}, Entertainment={entertainmentPriority}, Tourism={tourismPriority}");
        return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
    }
    
    
    // Un touched yet
    public async Task<Respond<List<TripPlanResponse>>> OptimizePlanMultiple(TripPlanRequest request)
    {
        var priorities = CalculatePriorities(request.Interests);
        Console.WriteLine($"Calculated Priorities: Accommodation={priorities.accommodation}, Food={priorities.food}, Entertainment={priorities.entertainment}, Tourism={priorities.tourism}");

        var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);
        Console.WriteLine($"Fetched Items: Total={allItems.Count}, Restaurants={allItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={allItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={allItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={allItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", allItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        // Filter items based on interests
        var desiredTypes = new HashSet<ItemType>();
        if (priorities.accommodation > 0) desiredTypes.Add(ItemType.Accommodation);
        if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
        if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
        if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
        var filteredItems = allItems.Where(i => desiredTypes.Contains(i.PlaceType)).ToList();
        Console.WriteLine($"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", filteredItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");

        var totalBudget = (int)(request.BudgetPerAdult);

        var constraints = new UserDefinedKnapsackConstraints(
            request.MaxRestaurants,
            request.MaxAccommodations,
            request.MaxEntertainments,
            request.MaxTourismAreas);

        var (maxProfit, allSolutions) = _solver.GetMaxProfitMultiple(totalBudget, filteredItems, constraints, priorities);
        var tripPlans = allSolutions.Select(items => BuildTripPlanResponse(items, request)).ToList();

        if (tripPlans.Any())
        {
            Console.WriteLine($"Generated {tripPlans.Count} trip plans with total items: {tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0))}");
            return new Respond<List<TripPlanResponse>>
            {
                Succeeded = true,
                Message = "Trip plans optimized successfully",
                Data = tripPlans,
                Meta = new { TotalItems = tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0)), TotalSolutions = allSolutions.Count }
            };
        }

        Console.WriteLine("No valid trip plans generated.");
        return new Respond<List<TripPlanResponse>>
        {
            Succeeded = false,
            Message = "No valid trip plans found",
            Errors = new List<string> { "Unable to generate solutions within constraints" }
        };
    }
}

// public partial class TripPlanOptimizer
// {
//     private readonly IKnapsackSolver _solver;
//     private readonly IItemFetcher _itemFetcher;
//     private readonly IStagedTripPlanOptimizer _stagedOptimizer;
//
//     public TripPlanOptimizer(IKnapsackSolver solver, IItemFetcher itemFetcher, IStagedTripPlanOptimizer stagedOptimizer)
//     {
//         _solver = solver;
//         _itemFetcher = itemFetcher;
//         _stagedOptimizer = stagedOptimizer;
//     }
//
// public async Task<Respond<TripPlanResponse>> OptimizePlan(TripPlanRequest request)
//         {
//             var priorities = CalculatePriorities(request.Interests);
//             Console.WriteLine($"Calculated Priorities: A={priorities.accommodation}, F={priorities.food}, E={priorities.entertainment}, T={priorities.tourism}");
//
//             var all = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);
//             var desired = all.Where(i => priorities switch
//             {
//                 var p when i.PlaceType == ItemType.Accommodation && p.accommodation > 0 => true,
//                 var p when i.PlaceType == ItemType.Restaurant && p.food > 0 => true,
//                 var p when i.PlaceType == ItemType.Entertainment && p.entertainment > 0 => true,
//                 var p when i.PlaceType == ItemType.TourismArea && p.tourism > 0 => true,
//                 _ => false
//             }).ToList();
//
//             var filtered = desired.Where(i => i.AveragePricePerAdult <= request.BudgetPerAdult && i.Score > 0).ToList();
//
//             var constraints = new UserDefinedKnapsackConstraints(
//                 request.MaxRestaurants,
//                 request.MaxAccommodations,
//                 request.MaxEntertainments,
//                 request.MaxTourismAreas);
//
//             var result = _stagedOptimizer.OptimizeStaged(
//                 filtered,
//                 new Queue<string>(request.Interests),
//                 (int)request.BudgetPerAdult,
//                 constraints,
//                 priorities);
//
//             var response = BuildTripPlanResponse(result, request);
//             if (result.Any())
//                 return new Respond<TripPlanResponse>(response, "Trip plan generated successfully")
//                 {
//                     Succeeded = true,
//                     StatusCode = System.Net.HttpStatusCode.OK
//                 };
//
//             return new Respond<TripPlanResponse>("No valid trip plan found")
//             {
//                 Succeeded = false,
//                 StatusCode = System.Net.HttpStatusCode.NotFound
//             };
//         }
//     // الدوال المساعدة (تم الاحتفاظ بها كما هي مع تعديل بسيط إذا لزم الأمر)
//     private ItemType GetItemType(string interest)
//     {
//         return interest?.Trim().ToLowerInvariant() switch
//         {
//             "accommodation" => ItemType.Accommodation,
//             "restaurants" or "food" => ItemType.Restaurant,
//             "entertainments" or "entertainment" => ItemType.Entertainment,
//             "tourismareas" or "tourism" => ItemType.TourismArea,
//             _ => ItemType.Restaurant // Default
//         };
//     }
//
//     private int GetMaxCount(ItemType itemType, IKnapsackConstraints constraints)
//     {
//         return itemType switch
//         {
//             ItemType.Restaurant => constraints.MaxRestaurants,
//             ItemType.Accommodation => constraints.MaxAccommodations,
//             ItemType.Entertainment => constraints.MaxEntertainments,
//             ItemType.TourismArea => constraints.MaxTourismAreas,
//             _ => 0
//         };
//     }
//
//     private TripPlanResponse BuildTripPlanResponse(List<Item> selectedItems, TripPlanRequest request)
//     {
//         var response = new TripPlanResponse
//         {
//             Accommodation = selectedItems.FirstOrDefault(i => i.PlaceType == ItemType.Accommodation)?.ToResponse(),
//             Restaurants = selectedItems.Where(i => i.PlaceType == ItemType.Restaurant)
//                 .Take(request.MaxRestaurants).Select(i => i.ToResponse()).ToList(),
//             Entertainments = selectedItems.Where(i => i.PlaceType == ItemType.Entertainment)
//                 .Take(request.MaxEntertainments).Select(i => i.ToResponse()).ToList(),
//             TourismAreas = selectedItems.Where(i => i.PlaceType == ItemType.TourismArea)
//                 .Take(request.MaxTourismAreas).Select(i => i.ToResponse()).ToList()
//         };
//
//         Console.WriteLine($"Built Response: Accommodation={(response.Accommodation?.Name ?? "None")}, Restaurants={response.Restaurants.Count}, Entertainments={response.Entertainments.Count}, TourismAreas={response.TourismAreas.Count}");
//         return response;
//     }
//
//     private (int accommodation, int food, int entertainment, int tourism) CalculatePriorities(Queue<string> interests)
//     {
//         int accommodationPriority = 0, foodPriority = 0, entertainmentPriority = 0, tourismPriority = 0;
//         int bonus = interests?.Count ?? 0;
//
//         Console.WriteLine($"Calculating priorities for interests: {string.Join(", ", interests ?? new Queue<string>())}");
//
//         if (interests != null)
//         {
//             foreach (var interest in interests)
//             {
//                 var normalizedInterest = interest?.Trim().ToLowerInvariant();
//                 Console.WriteLine($"Processing interest: {normalizedInterest}");
//                 switch (normalizedInterest)
//                 {
//                     case "accommodation":
//                         accommodationPriority = bonus--;
//                         break;
//                     case "restaurants":
//                     case "food":
//                         foodPriority = bonus--;
//                         break;
//                     case "entertainments":
//                     case "entertainment":
//                         entertainmentPriority = bonus--;
//                         break;
//                     case "tourismareas":
//                     case "tourism":
//                         tourismPriority = bonus--;
//                         break;
//                     default:
//                         Console.WriteLine($"Unrecognized interest: {normalizedInterest}");
//                         break;
//                 }
//             }
//         }
//
//         if ((interests?.Any() ?? false) && accommodationPriority == 0 && foodPriority == 0 && entertainmentPriority == 0 && tourismPriority == 0)
//         {
//             Console.WriteLine("No valid priorities set, defaulting to all priorities");
//             accommodationPriority = foodPriority = entertainmentPriority = tourismPriority = 1;
//         }
//
//         Console.WriteLine($"Calculated Priorities: Accommodation={accommodationPriority}, Food={foodPriority}, Entertainment={entertainmentPriority}, Tourism={tourismPriority}");
//         return (accommodationPriority, foodPriority, entertainmentPriority, tourismPriority);
//     }
//
//
//     // Un touched yet
//     public async Task<Respond<List<TripPlanResponse>>> OptimizePlanMultiple(TripPlanRequest request)
//     {
//         var priorities = CalculatePriorities(request.Interests);
//         Console.WriteLine($"Calculated Priorities: Accommodation={priorities.accommodation}, Food={priorities.food}, Entertainment={priorities.entertainment}, Tourism={priorities.tourism}");
//
//         var allItems = await _itemFetcher.FetchItems(request.GovernorateId, request.ZoneId, priorities);
//         Console.WriteLine($"Fetched Items: Total={allItems.Count}, Restaurants={allItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={allItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={allItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={allItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", allItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");
//
//         // Filter items based on interests
//         var desiredTypes = new HashSet<ItemType>();
//         if (priorities.accommodation > 0) desiredTypes.Add(ItemType.Accommodation);
//         if (priorities.food > 0) desiredTypes.Add(ItemType.Restaurant);
//         if (priorities.entertainment > 0) desiredTypes.Add(ItemType.Entertainment);
//         if (priorities.tourism > 0) desiredTypes.Add(ItemType.TourismArea);
//         var filteredItems = allItems.Where(i => desiredTypes.Contains(i.PlaceType)).ToList();
//         Console.WriteLine($"Filtered Items: Total={filteredItems.Count}, Restaurants={filteredItems.Count(i => i.PlaceType == ItemType.Restaurant)}, Accommodations={filteredItems.Count(i => i.PlaceType == ItemType.Accommodation)}, Entertainments={filteredItems.Count(i => i.PlaceType == ItemType.Entertainment)}, TourismAreas={filteredItems.Count(i => i.PlaceType == ItemType.TourismArea)}, GlobalIds={string.Join(", ", filteredItems.Select(i => $"{i.GlobalId} (Score={i.Score}, Price={i.AveragePricePerAdult})"))}");
//
//         var totalBudget = (int)(request.BudgetPerAdult);
//
//         var constraints = new UserDefinedKnapsackConstraints(
//             request.MaxRestaurants,
//             request.MaxAccommodations,
//             request.MaxEntertainments,
//             request.MaxTourismAreas);
//
//         var (maxProfit, allSolutions) = _solver.GetMaxProfitMultiple(totalBudget, filteredItems, constraints, priorities);
//         var tripPlans = allSolutions.Select(items => BuildTripPlanResponse(items, request)).ToList();
//
//         if (tripPlans.Any())
//         {
//             Console.WriteLine($"Generated {tripPlans.Count} trip plans with total items: {tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0))}");
//             return new Respond<List<TripPlanResponse>>
//             {
//                 Succeeded = true,
//                 Message = "Trip plans optimized successfully",
//                 Data = tripPlans,
//                 Meta = new { TotalItems = tripPlans.Sum(p => p.Restaurants.Count + p.Entertainments.Count + p.TourismAreas.Count + (p.Accommodation != null ? 1 : 0)), TotalSolutions = allSolutions.Count }
//             };
//         }
//
//         Console.WriteLine("No valid trip plans generated.");
//         return new Respond<List<TripPlanResponse>>
//         {
//             Succeeded = false,
//             Message = "No valid trip plans found",
//             Errors = new List<string> { "Unable to generate solutions within constraints" }
//         };
//     }
// }


// HELPER CLASSES
public record TripPlanRequest(
    int GovernorateId,
    int? ZoneId, 
    double BudgetPerAdult, 
    int NumberOfTravelers, 
    Queue<string> Interests, 
    int MaxRestaurants, 
    int MaxAccommodations, 
    int MaxEntertainments, 
    int MaxTourismAreas);
public record TripPlanResponse
{
    public ItemResponse Accommodation { get; init; }
    public List<ItemResponse> Restaurants { get; init; }
    public List<ItemResponse> Entertainments { get; init; }
    public List<ItemResponse> TourismAreas { get; init; }
}

public record ItemResponse(int Id, string Name, string ClassType, double AveragePricePerAdult, float Score, ItemType PlaceType, double Rating, string ImageSource);

public enum ItemType { Accommodation, Restaurant, Entertainment, TourismArea }

public class Item
{
    public int Id { get; set; }
    public string GlobalId { get; set; }
    public string Name { get; set; }
    public string ClassType { get; set; }
    public double AveragePricePerAdult { get; set; }
    public float Score { get; set; }
    public ItemType PlaceType { get; set; }
    public double Rating { get; set; }
    public string ImageSource { get; set; }
    public ItemResponse ToResponse() => new ItemResponse(Id, Name, ClassType, AveragePricePerAdult, Score, PlaceType, Rating, ImageSource);
}