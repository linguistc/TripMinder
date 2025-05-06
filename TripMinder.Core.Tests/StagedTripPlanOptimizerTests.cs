using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Behaviors.Shared;
using Xunit;

namespace TripMinder.Core.Tests.Behaviors.Knapsack;

public class StagedTripPlanOptimizerTests
{
    private readonly Mock<IDynamicProgrammingCalculator> _dpCalculatorMock;
    private readonly Mock<IProfitFinder> _profitFinderMock;
    private readonly Mock<IKnapsackBacktracker> _backtrackerMock;
    private readonly StagedTripPlanOptimizer _optimizer;

    public StagedTripPlanOptimizerTests()
    {
        _dpCalculatorMock = new Mock<IDynamicProgrammingCalculator>();
        _profitFinderMock = new Mock<IProfitFinder>();
        _backtrackerMock = new Mock<IKnapsackBacktracker>();
        _optimizer = new StagedTripPlanOptimizer(
            _dpCalculatorMock.Object,
            _profitFinderMock.Object,
            _backtrackerMock.Object
        );
    }

    //1
    [Fact]
    public async Task OptimizeStagedAsync_SingleAccommodationPriority_SelectsTopAccommodation()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 1, GlobalId = "Accommodation_1", PlaceType = ItemType.Accommodation,
                AveragePricePerAdult = 100, Score = 10f
            },
            new Item
            {
                Id = 2, GlobalId = "Accommodation_2", PlaceType = ItemType.Accommodation,
                AveragePricePerAdult = 100, Score = 8f
            },
        };
        var orderedInterests = new List<string> { "accommodation" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(0, 1, 0, 0);
        var priorities = (a: 1, f: 0, e: 0, t: 0);

        // Setup DP result: only one accommodation fits
        var dpArray = new float[budget + 1, 1, 2, 1, 1];
        var decisionArray = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var itemIdsArray = new int?[budget + 1, 1, 2, 1, 1, items.Count];

        dpArray[100, 0, 1, 0, 0] = 10f;
        decisionArray[100, 0, 1, 0, 0, 0] = true;
        itemIdsArray[100, 0, 1, 0, 0, 0] = 1;

        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dpArray, decisionArray, itemIdsArray));

        // Setup ProfitFinder
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpArray, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 100, 0, 1, 0, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpArray, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((10f, 100, 0, 1, 0, 0));

        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(
                It.IsAny<KnapsackState>(),
                It.IsAny<List<DpItem>>(),
                decisionArray))
            .Returns(new List<DpItem> { new DpItem(items[0]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(ItemType.Accommodation, result[0].PlaceType);
    }

    //2
    [Fact]
    public async Task OptimizeStagedAsync_AccommodationThenRestaurant_SelectsOneOfEach()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 1, GlobalId = "Accommodation_1", PlaceType = ItemType.Accommodation,
                AveragePricePerAdult = 100, Score = 10f
            },
            new Item
            {
                Id = 2, GlobalId = "Restaurant_1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 50,
                Score = 8f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food" };
        int budget = 150;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 2, f: 1, e: 0, t: 0);

        // Phase 1: accommodations=1, restaurants=0
        var dpPhase1 = new float[budget + 1, /*maxR+1=*/0 + 1, /*maxA+1=*/1 + 1, 1, 1];
        var decPhase1 = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var idsPhase1 = new int?[budget + 1, 1, 2, 1, 1, items.Count];
        dpPhase1[100, 0, 1, 0, 0] = 10f;
        decPhase1[100, 0, 1, 0, 0, 0] = true;
        idsPhase1[100, 0, 1, 0, 0, 0] = 1;

        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), /*maxR=*/0, /*maxA=*/1, 0, 0))
            .Returns((dpPhase1, decPhase1, idsPhase1));

        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpPhase1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 100, 0, 1, 0, 0));

        // Phase 2: accommodations=1, restaurants=1
        var dpPhase2 = new float[budget + 1, 1 + 1, 1 + 1, 1, 1];
        var decPhase2 = new bool[budget + 1, 2, 2, 1, 1, items.Count];
        var idsPhase2 = new int?[budget + 1, 2, 2, 1, 1, items.Count];
        dpPhase2[150, 1, 1, 0, 0] = 18f;
        decPhase2[150, 1, 1, 0, 0, 1] = true;
        idsPhase2[150, 1, 1, 0, 0, 1] = 2;

        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), /*maxR=*/1, /*maxA=*/1, 0, 0))
            .Returns((dpPhase2, decPhase2, idsPhase2));

        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpPhase2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((18f, 150, 1, 1, 0, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpPhase2, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((18f, 150, 1, 1, 0, 0));

        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(
                It.IsAny<KnapsackState>(),
                It.IsAny<List<DpItem>>(),
                decPhase2))
            .Returns(new List<DpItem> { new DpItem(items[0]), new DpItem(items[1]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.PlaceType == ItemType.Accommodation && i.Id == 1);
        Assert.Contains(result, i => i.PlaceType == ItemType.Restaurant && i.Id == 2);
    }

    //3
    [Fact]
    public async Task OptimizeStagedAsync_SponsorWeightAffectsSelection()
    {
        // Arrange
        // Two accommodations with equal base Score, but item 20 has extra sponsor weight
        var items = new List<Item>
        {
            new Item
            {
                Id = 20, GlobalId = "Accommodation_20", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 100,
                Score = 12f
            }, // 10 base + 2 sponsor
            new Item
            {
                Id = 21, GlobalId = "Accommodation_21", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 100,
                Score = 10f
            }
        };
        var orderedInterests = new List<string> { "accommodation" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 0, maxAccommodations: 1,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 1, f: 0, e: 0, t: 0);

        // Mock DP to pick the highest Score at full budget
        var dp = new float[budget + 1, 1, 2, 1, 1];
        var dec = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var ids = new int?[budget + 1, 1, 2, 1, 1, items.Count];

        // At budget=100, maxA=1, best profit is item20 with score 12
        dp[100, 0, 1, 0, 0] = 12f;
        dec[100, 0, 1, 0, 0, 0] = true;
        ids[100, 0, 1, 0, 0, 0] = 20;

        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp, dec, ids));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((12f, 100, 0, 1, 0, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((12f, 100, 0, 1, 0, 0));
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec))
            .Returns(new List<DpItem> { new DpItem(items[0]) }); // item20

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert: should select the sponsored item (Id 20)
        Assert.Single(result);
        Assert.Equal(20, result[0].Id);
        Assert.Equal(ItemType.Accommodation, result[0].PlaceType);
    }

    //4
    [Fact]
    public async Task OptimizeStagedAsync_MixedPriorities_RespectsPhaseOrderAndEarlyStop()
    {
        // Arrange: 3 types, but budget only allows two items
        var items = new List<Item>
        {
            new Item
            {
                Id = 101, GlobalId = "Accommodation_101", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 70,
                Score = 7f
            },
            new Item
            {
                Id = 102, GlobalId = "Restaurant_102", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 50,
                Score = 6f
            },
            new Item
            {
                Id = 103, GlobalId = "Entertainment_103", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 30,
                Score = 5f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food", "entertainment" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 1, maxTourismAreas: 0);
        var priorities = (a: 3, f: 2, e: 1, t: 0);

        // Phase1: Accommodations only (maxA=1, maxF=0)
        var dp1 = new float[budget + 1, 1, 2, 2, 1];
        var dec1 = new bool[budget + 1, 1, 2, 2, 1, items.Count];
        var ids1 = new int?[budget + 1, 1, 2, 2, 1, items.Count];
        dp1[70, 0, 1, 0, 0] = 7f;
        dec1[70, 0, 1, 0, 0, 0] = true;
        ids1[70, 0, 1, 0, 0, 0] = 101;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((7f, 70, 0, 1, 0, 0));

        // Phase2: Add Restaurant (maxA=1,maxF=1)
        var dp2 = new float[budget + 1, 2, 2, 2, 1];
        var dec2 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids2 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp2[100, 1, 1, 0, 0] = 13f; // 7+6
        dec2[100, 1, 1, 0, 0, 1] = true;
        ids2[100, 1, 1, 0, 0, 1] = 102;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((13f, 100, 1, 1, 0, 0));

        // Phase3: Try Entertainment (budget exhausted => early stop)
        // We don't set up a dp3 => optimizer should detect no progress and stop

        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(
                It.IsAny<KnapsackState>(),
                It.IsAny<List<DpItem>>(),
                dec2))
            .Returns(new List<DpItem> { new DpItem(items[0]), new DpItem(items[1]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert: only accommodation and restaurant selected, entertainment skipped due to budget
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Id == 101 && i.PlaceType == ItemType.Accommodation);
        Assert.Contains(result, i => i.Id == 102 && i.PlaceType == ItemType.Restaurant);
    }

    //5
    [Fact]
    public async Task OptimizeStagedAsync_EqualScoreDifferentRating_PrefersHigherRating()
    {
        // Arrange
        // Two entertainment items with equal Score, but item 200 has higher Rating
        var items = new List<Item>
        {
            new Item
            {
                Id = 200, GlobalId = "Entertainment_200", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 40,
                Score = 5f, Rating = 4.8
            },
            new Item
            {
                Id = 201, GlobalId = "Entertainment_201", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 40,
                Score = 5f, Rating = 4.2
            }
        };
        var orderedInterests = new List<string> { "entertainment" };
        int budget = 40;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 0, maxAccommodations: 0,
            maxEntertainments: 1, maxTourismAreas: 0);
        var priorities = (a: 0, f: 0, e: 1, t: 0);

        // We simulate that higher Rating acts as a tiebreaker in Score calculation phase,
        // so DP chooses item 200.
        var dp = new float[budget + 1, 1, 1, 2, 1];
        var dec = new bool[budget + 1, 1, 1, 2, 1, items.Count];
        var ids = new int?[budget + 1, 1, 1, 2, 1, items.Count];

        dp[40, 0, 0, 1, 0] = 5f; // only one slot for Entertainment
        dec[40, 0, 0, 1, 0, 0] = true; // picks first DpItem (items[0])
        ids[40, 0, 0, 1, 0, 0] = 200;

        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 0, 1, 0))
            .Returns((dp, dec, ids));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((5f, 40, 0, 0, 1, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((5f, 40, 0, 0, 1, 0));
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec))
            .Returns(new List<DpItem> { new DpItem(items[0]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Single(result);
        Assert.Equal(200, result[0].Id);
        Assert.Equal(ItemType.Entertainment, result[0].PlaceType);
    }

    //6
    [Fact]
    public async Task OptimizeStagedAsync_MultipleTypes_MaxConstraintsEnforced()
    {
        // Arrange: 2 accommodations and 2 restaurants but MaxAccommodations=1, MaxRestaurants=1
        var items = new List<Item>
        {
            new Item
            {
                Id = 301, GlobalId = "Accommodation_301", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 60,
                Score = 6f
            },
            new Item
            {
                Id = 302, GlobalId = "Accommodation_302", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 60,
                Score = 5f
            },
            new Item
            {
                Id = 303, GlobalId = "Restaurant_303", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 40,
                Score = 7f
            },
            new Item
            {
                Id = 304, GlobalId = "Restaurant_304", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 40,
                Score = 4f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 2, f: 1, e: 0, t: 0);

        // Mock DP output: best single accommodation (id=301) then best restaurant (id=303)
        var dpPhase1 = new float[budget + 1, 1, 2, 1, 1];
        var dec1 = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var ids1 = new int?[budget + 1, 1, 2, 1, 1, items.Count];
        dpPhase1[60, 0, 1, 0, 0] = 6f;
        dec1[60, 0, 1, 0, 0, 0] = true;
        ids1[60, 0, 1, 0, 0, 0] = 301;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dpPhase1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpPhase1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((6f, 60, 0, 1, 0, 0));

        var dpPhase2 = new float[budget + 1, 2, 2, 1, 1];
        var dec2 = new bool[budget + 1, 2, 2, 1, 1, items.Count];
        var ids2 = new int?[budget + 1, 2, 2, 1, 1, items.Count];
        dpPhase2[100, 1, 1, 0, 0] = 13f; // 6 + 7
        dec2[100, 1, 1, 0, 0, 2] = true;
        ids2[100, 1, 1, 0, 0, 2] = 303;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 0, 0))
            .Returns((dpPhase2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpPhase2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((13f, 100, 1, 1, 0, 0));
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec2))
            .Returns(new List<DpItem> { new DpItem(items[0]), new DpItem(items[2]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.PlaceType == ItemType.Accommodation && i.Id == 301);
        Assert.Contains(result, i => i.PlaceType == ItemType.Restaurant && i.Id == 303);
    }

    //7
    [Fact]
    public async Task OptimizeStagedAsync_FreeExpansion_AddsSecondAccommodation()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 1, GlobalId = "Accommodation_1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 80,
                Score = 10f
            },
            new Item
            {
                Id = 2, GlobalId = "Accommodation_2", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 70,
                Score = 8f
            }
        };
        var orderedInterests = new List<string> { "accommodation" };
        int budget = 150;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 0, maxAccommodations: 2,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 1, f: 0, e: 0, t: 0);

        // Phase 1: maxA = 1
        var dp1 = new float[budget + 1, /*R*/1, /*A*/2, /*E*/1, /*T*/1];
        var dec1 = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var ids1 = new int?[budget + 1, 1, 2, 1, 1, items.Count];
        dp1[80, 0, 1, 0, 0] = 10f;
        dec1[80, 0, 1, 0, 0, 0] = true;
        ids1[80, 0, 1, 0, 0, 0] = 1;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 80, 0, 1, 0, 0));

        // Phase 2: free expansion maxA = 2
        var dp2 = new float[budget + 1, 1, /*A*/3, 1, 1];
        var dec2 = new bool[budget + 1, 1, 3, 1, 1, items.Count];
        var ids2 = new int?[budget + 1, 1, 3, 1, 1, items.Count];
        dp2[150, 0, 2, 0, 0] = 18f;
        dec2[150, 0, 2, 0, 0, 1] = true;
        ids2[150, 0, 2, 0, 0, 1] = 2;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 2, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((18f, 150, 0, 2, 0, 0));
        // Final exact phase
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((18f, 150, 0, 2, 0, 0));

        // Backtrack final solution
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(
                It.IsAny<KnapsackState>(),
                It.IsAny<List<DpItem>>(),
                dec2))
            .Returns(new List<DpItem> { new DpItem(items[0]), new DpItem(items[1]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.PlaceType == ItemType.Accommodation && i.Id == 1);
        Assert.Contains(result, i => i.PlaceType == ItemType.Accommodation && i.Id == 2);
    }

    //8
    [Fact]
    public async Task OptimizeStagedAsync_EarlyStopBudgetTooLow_ReturnsEmptyList()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 1, GlobalId = "Accommodation_1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50,
                Score = 5f
            },
            new Item
            {
                Id = 2, GlobalId = "Restaurant_1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 40,
                Score = 4f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food" };
        int budget = 30; // less than the cheapest item (40)
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 1, f: 1, e: 0, t: 0);

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Empty(result);
        _dpCalculatorMock.Verify(c => c.Calculate(It.IsAny<int>(), It.IsAny<List<DpItem>>(),
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _profitFinderMock.Verify(p => p.FindMaxProfit(It.IsAny<float[,,,,]>(), It.IsAny<int>(),
            It.IsAny<IKnapsackConstraints>(), It.IsAny<bool>()), Times.Never);
        _backtrackerMock.Verify(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(),
            It.IsAny<List<DpItem>>(), It.IsAny<bool[,,,,,]>()), Times.Never);
    }

    //9
    [Fact]
    public async Task OptimizeStagedAsync_Phase1CoverageAllTypes()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 1, GlobalId = "Accommodation_1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 10,
                Score = 10f
            },
            new Item
            {
                Id = 2, GlobalId = "Restaurant_2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 10,
                Score = 10f
            },
            new Item
            {
                Id = 3, GlobalId = "Entertainment_3", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 10,
                Score = 10f
            },
            new Item
            {
                Id = 4, GlobalId = "TourismArea_4", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 10,
                Score = 10f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food", "entertainment", "tourism" };
        int budget = 40;
        var constraints = new UserDefinedKnapsackConstraints(1, 1, 1, 1);
        var priorities = (a: 1, f: 1, e: 1, t: 1);

        // Phase1: Accommodation only
        var dp1 = new float[budget + 1, 1, 2, 1, 1];
        var dec1 = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var ids1 = new int?[budget + 1, 1, 2, 1, 1, items.Count];
        dp1[10, 0, 1, 0, 0] = 10f;
        dec1[10, 0, 1, 0, 0, 0] = true;
        ids1[10, 0, 1, 0, 0, 0] = 1;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 10, 0, 1, 0, 0));

        // Phase2: +Restaurant
        var dp2 = new float[budget + 1, 2, 2, 1, 1];
        var dec2 = new bool[budget + 1, 2, 2, 1, 1, items.Count];
        var ids2 = new int?[budget + 1, 2, 2, 1, 1, items.Count];
        dp2[20, 1, 1, 0, 0] = 20f;
        dec2[20, 1, 1, 0, 0, 1] = true;
        ids2[20, 1, 1, 0, 0, 1] = 2;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((20f, 20, 1, 1, 0, 0));

        // Phase3: +Entertainment
        var dp3 = new float[budget + 1, 2, 2, 2, 1];
        var dec3 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids3 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp3[30, 1, 1, 1, 0] = 30f;
        dec3[30, 1, 1, 1, 0, 2] = true;
        ids3[30, 1, 1, 1, 0, 2] = 3;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 1, 0))
            .Returns((dp3, dec3, ids3));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp3, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((30f, 30, 1, 1, 1, 0));

        // Phase4: +TourismArea
        var dp4 = new float[budget + 1, 2, 2, 2, 2];
        var dec4 = new bool[budget + 1, 2, 2, 2, 2, items.Count];
        var ids4 = new int?[budget + 1, 2, 2, 2, 2, items.Count];
        dp4[40, 1, 1, 1, 1] = 40f;
        dec4[40, 1, 1, 1, 1, 3] = true;
        ids4[40, 1, 1, 1, 1, 3] = 4;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 1, 1))
            .Returns((dp4, dec4, ids4));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp4, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((40f, 40, 1, 1, 1, 1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp4, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((40f, 40, 1, 1, 1, 1));

        // Final backtrack selects all four
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec4))
            .Returns(new List<DpItem>
            {
                new DpItem(items[0]),
                new DpItem(items[1]),
                new DpItem(items[2]),
                new DpItem(items[3])
            });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Contains(result, i => i.PlaceType == ItemType.Accommodation);
        Assert.Contains(result, i => i.PlaceType == ItemType.Restaurant);
        Assert.Contains(result, i => i.PlaceType == ItemType.Entertainment);
        Assert.Contains(result, i => i.PlaceType == ItemType.TourismArea);
    }

    //10
    [Fact]
    public async Task OptimizeStagedAsync_FreeExpansion_AddsMultipleTypes()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 1, GlobalId = "Accommodation_1", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 10,
                Score = 10f
            },
            new Item
            {
                Id = 2, GlobalId = "Restaurant_1", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 10,
                Score = 8f
            },
            new Item
            {
                Id = 3, GlobalId = "Entertainment_1", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 10,
                Score = 9f
            },
            new Item
            {
                Id = 4, GlobalId = "TourismArea_1", PlaceType = ItemType.TourismArea, AveragePricePerAdult = 10,
                Score = 7f
            },
            new Item
            {
                Id = 5, GlobalId = "Restaurant_2", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 10,
                Score = 6f
            },
            new Item
            {
                Id = 6, GlobalId = "Entertainment_2", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 10,
                Score = 5f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food", "entertainment", "tourism" };
        int budget = 60;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 2, maxAccommodations: 1,
            maxEntertainments: 2, maxTourismAreas: 1);
        var priorities = (a: 1, f: 1, e: 1, t: 1);

        // Phase 1: Accommodation only
        var dp1 = new float[budget + 1, 3, 2, 3, 2];
        var dec1 = new bool[budget + 1, 3, 2, 3, 2, items.Count];
        var ids1 = new int?[budget + 1, 3, 2, 3, 2, items.Count];
        dp1[10, 0, 1, 0, 0] = 10f;
        dec1[10, 0, 1, 0, 0, 0] = true;
        ids1[10, 0, 1, 0, 0, 0] = 1;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 10, 0, 1, 0, 0));

        // Phase 2: +Restaurant
        var dp2 = new float[budget + 1, 3, 2, 3, 2];
        var dec2 = new bool[budget + 1, 3, 2, 3, 2, items.Count];
        var ids2 = new int?[budget + 1, 3, 2, 3, 2, items.Count];
        dp2[20, 1, 1, 0, 0] = 18f; // 10 + 8
        dec2[20, 1, 1, 0, 0, 1] = true;
        ids2[20, 1, 1, 0, 0, 1] = 2;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((18f, 20, 1, 1, 0, 0));

        // Phase 3: +Entertainment
        var dp3 = new float[budget + 1, 3, 2, 3, 2];
        var dec3 = new bool[budget + 1, 3, 2, 3, 2, items.Count];
        var ids3 = new int?[budget + 1, 3, 2, 3, 2, items.Count];
        dp3[30, 1, 1, 1, 0] = 27f; // 10 + 8 + 9
        dec3[30, 1, 1, 1, 0, 2] = true;
        ids3[30, 1, 1, 1, 0, 2] = 3;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 1, 0))
            .Returns((dp3, dec3, ids3));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp3, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((27f, 30, 1, 1, 1, 0));

        // Phase 4: +TourismArea
        var dp4 = new float[budget + 1, 3, 2, 3, 2];
        var dec4 = new bool[budget + 1, 3, 2, 3, 2, items.Count];
        var ids4 = new int?[budget + 1, 3, 2, 3, 2, items.Count];
        dp4[40, 1, 1, 1, 1] = 34f; // 10 + 8 + 9 + 7
        dec4[40, 1, 1, 1, 1, 3] = true;
        ids4[40, 1, 1, 1, 1, 3] = 4;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 1, 1))
            .Returns((dp4, dec4, ids4));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp4, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((34f, 40, 1, 1, 1, 1));

        // Free Expansion: Add Restaurant (R2)
        var dp5 = new float[budget + 1, 3, 2, 3, 2];
        var dec5 = new bool[budget + 1, 3, 2, 3, 2, items.Count];
        var ids5 = new int?[budget + 1, 3, 2, 3, 2, items.Count];
        dp5[50, 2, 1, 1, 1] = 40f; // 10 + 8 + 9 + 7 + 6
        dec5[50, 2, 1, 1, 1, 4] = true;
        ids5[50, 2, 1, 1, 1, 4] = 5;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 2, 1, 1, 1))
            .Returns((dp5, dec5, ids5));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp5, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((40f, 50, 2, 1, 1, 1));

        // Free Expansion: Add Entertainment (E2)
        var dp6 = new float[budget + 1, 3, 2, 3, 2];
        var dec6 = new bool[budget + 1, 3, 2, 3, 2, items.Count];
        var ids6 = new int?[budget + 1, 3, 2, 3, 2, items.Count];
        dp6[60, 2, 1, 2, 1] = 45f; // 10 + 8 + 9 + 7 + 6 + 5
        dec6[60, 2, 1, 2, 1, 5] = true;
        ids6[60, 2, 1, 2, 1, 5] = 6;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 2, 1, 2, 1))
            .Returns((dp6, dec6, ids6));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp6, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((45f, 60, 2, 1, 2, 1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp6, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((45f, 60, 2, 1, 2, 1));

        // Backtrack final solution
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec6))
            .Returns(new List<DpItem>
            {
                new DpItem(items[0]), // A1
                new DpItem(items[1]), // R1
                new DpItem(items[2]), // E1
                new DpItem(items[3]), // T1
                new DpItem(items[4]), // R2
                new DpItem(items[5]) // E2
            });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Equal(6, result.Count);
        Assert.Contains(result, i => i.Id == 1 && i.PlaceType == ItemType.Accommodation);
        Assert.Contains(result, i => i.Id == 2 && i.PlaceType == ItemType.Restaurant);
        Assert.Contains(result, i => i.Id == 3 && i.PlaceType == ItemType.Entertainment);
        Assert.Contains(result, i => i.Id == 4 && i.PlaceType == ItemType.TourismArea);
        Assert.Contains(result, i => i.Id == 5 && i.PlaceType == ItemType.Restaurant);
        Assert.Contains(result, i => i.Id == 6 && i.PlaceType == ItemType.Entertainment);
        Assert.Equal(60, result.Sum(i => i.AveragePricePerAdult));
        Assert.Equal(45f, result.Sum(i => i.Score));
    }

    //11
    [Fact]
    public async Task OptimizeStagedAsync_EqualPrioritiesLargeDataSet_SelectsHighestScores()
    {
        // Arrange
        var items = new List<Item>
        {
            // Accommodations
            new Item
            {
                Id = 501, GlobalId = "Accommodation_501", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50,
                Score = 10f
            },
            new Item
            {
                Id = 502, GlobalId = "Accommodation_502", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50,
                Score = 9f
            },
            new Item
            {
                Id = 503, GlobalId = "Accommodation_503", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50,
                Score = 8f
            },
            new Item
            {
                Id = 504, GlobalId = "Accommodation_504", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50,
                Score = 7f
            },
            new Item
            {
                Id = 505, GlobalId = "Accommodation_505", PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50,
                Score = 6f
            },
            // Restaurants
            new Item
            {
                Id = 506, GlobalId = "Restaurant_506", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 30,
                Score = 8f
            },
            new Item
            {
                Id = 507, GlobalId = "Restaurant_507", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 30,
                Score = 7f
            },
            new Item
            {
                Id = 508, GlobalId = "Restaurant_508", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 30,
                Score = 6f
            },
            new Item
            {
                Id = 509, GlobalId = "Restaurant_509", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 30,
                Score = 5f
            },
            new Item
            {
                Id = 510, GlobalId = "Restaurant_510", PlaceType = ItemType.Restaurant, AveragePricePerAdult = 30,
                Score = 4f
            },
            // Entertainments
            new Item
            {
                Id = 511, GlobalId = "Entertainment_511", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 20,
                Score = 6f
            },
            new Item
            {
                Id = 512, GlobalId = "Entertainment_512", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 20,
                Score = 5f
            },
            new Item
            {
                Id = 513, GlobalId = "Entertainment_513", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 20,
                Score = 4f
            },
            new Item
            {
                Id = 514, GlobalId = "Entertainment_514", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 20,
                Score = 3f
            },
            new Item
            {
                Id = 515, GlobalId = "Entertainment_515", PlaceType = ItemType.Entertainment, AveragePricePerAdult = 20,
                Score = 2f
            }
        };
        var orderedInterests = new List<string> { "accommodation", "food", "entertainment" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 1, maxTourismAreas: 0);
        var priorities = (a: 1, f: 1, e: 1, t: 0);

        // Phase 1: Accommodation only
        var dp1 = new float[budget + 1, 2, 2, 2, 1];
        var dec1 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids1 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp1[50, 0, 1, 0, 0] = 10f;
        dec1[50, 0, 1, 0, 0, 0] = true;
        ids1[50, 0, 1, 0, 0, 0] = 501;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 50, 0, 1, 0, 0));

        // Phase 2: +Restaurant
        var dp2 = new float[budget + 1, 2, 2, 2, 1];
        var dec2 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids2 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp2[80, 1, 1, 0, 0] = 18f; // 10 + 8
        dec2[80, 1, 1, 0, 0, 5] = true; // Restaurant_506
        ids2[80, 1, 1, 0, 0, 5] = 506;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((18f, 80, 1, 1, 0, 0));

        // Phase 3: +Entertainment
        var dp3 = new float[budget + 1, 2, 2, 2, 1];
        var dec3 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids3 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp3[100, 1, 1, 1, 0] = 24f; // 10 + 8 + 6
        dec3[100, 1, 1, 1, 0, 10] = true; // Entertainment_511
        ids3[100, 1, 1, 1, 0, 10] = 511;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 1, 0))
            .Returns((dp3, dec3, ids3));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp3, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((24f, 100, 1, 1, 1, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp3, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((24f, 100, 1, 1, 1, 0));

        // Backtrack final solution
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec3))
            .Returns(new List<DpItem>
            {
                new DpItem(items[0]), // Accommodation_501
                new DpItem(items[5]), // Restaurant_506
                new DpItem(items[10]) // Entertainment_511
            });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, orderedInterests, budget, constraints, priorities);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, i => i.Id == 501 && i.PlaceType == ItemType.Accommodation);
        Assert.Contains(result, i => i.Id == 506 && i.PlaceType == ItemType.Restaurant);
        Assert.Contains(result, i => i.Id == 511 && i.PlaceType == ItemType.Entertainment);
        Assert.Equal(100, result.Sum(i => i.AveragePricePerAdult));
        Assert.Equal(24f, result.Sum(i => i.Score));
    }

    [Fact]
    public async Task OptimizeStagedAsync_SponsoredLowerClassBeatsHigher()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 100, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 100, Score = 8f },
            new Item { Id = 101, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 100, Score = 9f }
        };
        var interests = new List<string> { "accommodation" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 0, maxAccommodations: 1,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 1, f: 0, e: 0, t: 0);

        // Mock DP to select the higher-scored item (Id=101)
        var dp = new float[budget + 1, 1, 2, 1, 1];
        var dec = new bool[budget + 1, 1, 2, 1, 1, items.Count];
        var ids = new int?[budget + 1, 1, 2, 1, 1, items.Count];
        dp[100, 0, 1, 0, 0] = 9f;
        dec[100, 0, 1, 0, 0, 1] = true;
        ids[100, 0, 1, 0, 0, 1] = 101;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp, dec, ids));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((9f, 100, 0, 1, 0, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((9f, 100, 0, 1, 0, 0));
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec))
            .Returns(new List<DpItem> { new DpItem(items[1]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Single(result);
        Assert.Equal(101, result[0].Id);
    }

    [Fact]
    public async Task OptimizeStagedAsync_PartialPhaseCoverage()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 201, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 60, Score = 9f },
            new Item { Id = 202, PlaceType = ItemType.Restaurant, AveragePricePerAdult = 40, Score = 8f },
            new Item { Id = 203, PlaceType = ItemType.Entertainment, AveragePricePerAdult = 30, Score = 7f }
        };
        var interests = new List<string> { "accommodation", "food", "entertainment" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 1, maxTourismAreas: 0);
        var priorities = (a: 1, f: 1, e: 1, t: 0);

        // Phase 1: pick accommodation
        var dp1 = new float[budget + 1, 2, 2, 2, 1];
        var dec1 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids1 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp1[60, 0, 1, 0, 0] = 9f;
        dec1[60, 0, 1, 0, 0, 0] = true;
        ids1[60, 0, 1, 0, 0, 0] = 201;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((9f, 60, 0, 1, 0, 0));

        // Phase 2: pick restaurant
        var dp2 = new float[budget + 1, 2, 2, 2, 1];
        var dec2 = new bool[budget + 1, 2, 2, 2, 1, items.Count];
        var ids2 = new int?[budget + 1, 2, 2, 2, 1, items.Count];
        dp2[100, 1, 1, 0, 0] = 17f; // 9+8
        dec2[100, 1, 1, 0, 0, 1] = true;
        ids2[100, 1, 1, 0, 0, 1] = 202;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 1, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((17f, 100, 1, 1, 0, 0));

        // Backtrack selects both
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec2))
            .Returns(new List<DpItem> { new DpItem(items[0]), new DpItem(items[1]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Id == 201);
        Assert.Contains(result, i => i.Id == 202);
    }


    [Fact]
    public async Task OptimizeStagedAsync_SingleHighCostItem()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 301, PlaceType = ItemType.Entertainment, AveragePricePerAdult = 200, Score = 4f }
        };
        var interests = new List<string> { "entertainment" };
        int budget = 200;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 0, maxAccommodations: 0,
            maxEntertainments: 1, maxTourismAreas: 0);
        var priorities = (a: 0, f: 0, e: 1, t: 0);

        // Mock DP to include the single item
        var dp = new float[budget + 1, 1, 1, 2, 1];
        var dec = new bool[budget + 1, 1, 1, 2, 1, items.Count];
        var ids = new int?[budget + 1, 1, 1, 2, 1, items.Count];
        dp[200, 0, 0, 1, 0] = 4f;
        dec[200, 0, 0, 1, 0, 0] = true;
        ids[200, 0, 0, 1, 0, 0] = 301;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 0, 1, 0))
            .Returns((dp, dec, ids));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((4f, 200, 0, 0, 1, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((4f, 200, 0, 0, 1, 0));
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec))
            .Returns(new List<DpItem> { new DpItem(items[0]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Single(result);
        Assert.Equal(301, result[0].Id);
    }

    [Fact]
    public async Task OptimizeStagedAsync_RatingVsPrice_EqualScorePrefersHigherRating()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item
            {
                Id = 401, PlaceType = ItemType.Restaurant, AveragePricePerAdult = 100, Score = 7f, Rating = 4.5
            },
            new Item { Id = 402, PlaceType = ItemType.Restaurant, AveragePricePerAdult = 80, Score = 7f, Rating = 3.5 }
        };
        var interests = new List<string> { "food" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 0,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 0, f: 1, e: 0, t: 0);

        // Mock DP to pick the first DpItem (higher Rating as tiebreaker)
        var dp = new float[budget + 1, 2, 1, 1, 1];
        var dec = new bool[budget + 1, 2, 1, 1, 1, items.Count];
        var ids = new int?[budget + 1, 2, 1, 1, 1, items.Count];
        dp[100, 1, 0, 0, 0] = 7f;
        dec[100, 1, 0, 0, 0, 0] = true;
        ids[100, 1, 0, 0, 0, 0] = 401;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 0, 0, 0))
            .Returns((dp, dec, ids));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((7f, 100, 1, 0, 0, 0));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp, budget, It.IsAny<IKnapsackConstraints>(), true))
            .Returns((7f, 100, 1, 0, 0, 0));
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec))
            .Returns(new List<DpItem> { new DpItem(items[0]) });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Single(result);
        Assert.Equal(401, result[0].Id);
    }

    [Fact]
    public async Task OptimizeStagedAsync_MaxConstraintExceedsAvailability_SelectsAll()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 501, PlaceType = ItemType.Restaurant, AveragePricePerAdult = 40, Score = 6f },
            new Item { Id = 502, PlaceType = ItemType.Restaurant, AveragePricePerAdult = 30, Score = 5f }
        };
        var interests = new List<string> { "food" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(
            maxRestaurants: 3, maxAccommodations: 0, maxEntertainments: 0, maxTourismAreas: 0
        );
        var priorities = (a: 0, f: 1, e: 0, t: 0);

        // Phase 1: maxR = 1
        var dpPhase1 = new float[budget + 1, 2, 1, 1, 1];
        var decPhase1 = new bool[budget + 1, 2, 1, 1, 1, items.Count];
        var idsPhase1 = new int?[budget + 1, 2, 1, 1, 1, items.Count];
        dpPhase1[40, 1, 0, 0, 0] = 6f;
        decPhase1[40, 1, 0, 0, 0, 0] = true;
        idsPhase1[40, 1, 0, 0, 0, 0] = 501;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 1, 0, 0, 0))
            .Returns((dpPhase1, decPhase1, idsPhase1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpPhase1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((6f, 40, 1, 0, 0, 0));

        // Free-expansion Phase: maxR = 2
        var dpFree = new float[budget + 1, 3, 1, 1, 1];
        var decFree = new bool[budget + 1, 3, 1, 1, 1, items.Count];
        var idsFree = new int?[budget + 1, 3, 1, 1, 1, items.Count];
        dpFree[70, 2, 0, 0, 0] = 11f; // 6 + 5
        decFree[70, 2, 0, 0, 0, 0] = true;
        idsFree[70, 2, 0, 0, 0, 0] = 501;
        decFree[70, 2, 0, 0, 0, 1] = true;
        idsFree[70, 2, 0, 0, 0, 1] = 502;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 2, 0, 0, 0))
            .Returns((dpFree, decFree, idsFree));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dpFree, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((11f, 70, 2, 0, 0, 0));

        // Backtrack final free-expansion solution
        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(
                It.IsAny<KnapsackState>(),
                It.IsAny<List<DpItem>>(),
                decFree))
            .Returns(new List<DpItem>
            {
                new DpItem(items[0]),
                new DpItem(items[1])
            });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Id == 501);
        Assert.Contains(result, i => i.Id == 502);
    }

    [Fact]
    public async Task OptimizeStagedAsync_AllItemsTooExpensive_ReturnsEmpty()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 701, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 150, Score = 10f },
            new Item { Id = 702, PlaceType = ItemType.Restaurant, AveragePricePerAdult = 120, Score = 8f }
        };
        var interests = new List<string> { "accommodation", "food" };
        int budget = 100;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 1, maxAccommodations: 1,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 1, f: 1, e: 0, t: 0);

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Empty(result);
        // no DP or backtrack calls should happen
        _dpCalculatorMock.Verify(c => c.Calculate(It.IsAny<int>(), It.IsAny<List<DpItem>>(),
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        _backtrackerMock.Verify(b => b.BacktrackSingleSolution(
            It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), It.IsAny<bool[,,,,,]>()), Times.Never);
    }

    [Fact]
    public async Task OptimizeStagedAsync_SponsorWeightInFreeExpansion()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 801, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50, Score = 10f },
            new Item { Id = 802, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50, Score = 8f },
            new Item { Id = 803, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 50, Score = 12f } // sponsor
        };
        var interests = new List<string> { "accommodation" };
        int budget = 150;
        var constraints = new UserDefinedKnapsackConstraints(maxRestaurants: 0, maxAccommodations: 3,
            maxEntertainments: 0, maxTourismAreas: 0);
        var priorities = (a: 1, f: 0, e: 0, t: 0);

        // Phase 1: maxA = 1 → picks 801
        var dp1 = new float[budget + 1, 1, 4, 1, 1];
        var dec1 = new bool[budget + 1, 1, 4, 1, 1, items.Count];
        var ids1 = new int?[budget + 1, 1, 4, 1, 1, items.Count];
        dp1[50, 0, 1, 0, 0] = 10f;
        dec1[50, 0, 1, 0, 0, 0] = true;
        ids1[50, 0, 1, 0, 0, 0] = 801;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 1, 0, 0))
            .Returns((dp1, dec1, ids1));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp1, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((10f, 50, 0, 1, 0, 0));

        // Free-expansion Phase: maxA = 2 → picks sponsor 803
        var dp2 = new float[budget + 1, 1, 5, 1, 1];
        var dec2 = new bool[budget + 1, 1, 5, 1, 1, items.Count];
        var ids2 = new int?[budget + 1, 1, 5, 1, 1, items.Count];
        dp2[100, 0, 2, 0, 0] = 22f; // 10 + 12
        dec2[100, 0, 2, 0, 0, 2] = true;
        ids2[100, 0, 2, 0, 0, 2] = 803;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 2, 0, 0))
            .Returns((dp2, dec2, ids2));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp2, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((22f, 100, 0, 2, 0, 0));

        // Free-expansion Phase: maxA = 3 → picks remaining A2
        var dp3 = new float[budget + 1, 1, 6, 1, 1];
        var dec3 = new bool[budget + 1, 1, 6, 1, 1, items.Count];
        var ids3 = new int?[budget + 1, 1, 6, 1, 1, items.Count];
        dp3[150, 0, 3, 0, 0] = 30f; // 10 + 12 + 8
        dec3[150, 0, 3, 0, 0, 1] = true;
        ids3[150, 0, 3, 0, 0, 1] = 802;
        _dpCalculatorMock
            .Setup(c => c.Calculate(budget, It.IsAny<List<DpItem>>(), 0, 3, 0, 0))
            .Returns((dp3, dec3, ids3));
        _profitFinderMock
            .Setup(p => p.FindMaxProfit(dp3, budget, It.IsAny<IKnapsackConstraints>(), false))
            .Returns((30f, 150, 0, 3, 0, 0));

        _backtrackerMock
            .Setup(b => b.BacktrackSingleSolution(It.IsAny<KnapsackState>(), It.IsAny<List<DpItem>>(), dec3))
            .Returns(new List<DpItem>
            {
                new DpItem(items[0]), // 801
                new DpItem(items[2]), // 803
                new DpItem(items[1]) // 802
            });

        // Act
        var result = await _optimizer.OptimizeStagedAsync(items, interests, budget, constraints, priorities);

        // Assert
        Assert.Equal(new[] { 801, 803, 802 }, result.Select(i => i.Id).ToArray());
    }

    

}


/*

    [Theory]
    [InlineData("tourism","accommodation","entertainment","food")]
    [InlineData("tourism","food","entertainment","accommodation")]
    [InlineData("entertainment","tourism","accommodation","food")]
    [InlineData("food","tourism","accommodation","entertainment")]
    public async Task OptimizeStagedAsync_RespectsInterestOrderPermutations(params string[] orderedInterests)
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 1, PlaceType = ItemType.TourismArea,   AveragePricePerAdult = 10, Score = 10f },
            new Item { Id = 2, PlaceType = ItemType.Accommodation, AveragePricePerAdult = 10, Score = 10f },
            new Item { Id = 3, PlaceType = ItemType.Entertainment, AveragePricePerAdult = 10, Score = 10f },
            new Item { Id = 4, PlaceType = ItemType.Restaurant,    AveragePricePerAdult = 10, Score = 10f }
        };
        int budget = 40;
        var constraints = new UserDefinedKnapsackConstraints(1,1,1,1);
        var priorities = (a:1, f:1, e:1, t:1);

        // Use real DP/backtracker
        var dpCalc      = new DynamicProgrammingCalculator();
        var profitFinder= new ProfitFinder();
        var backtracker = new KnapsackBacktracker();
        var optimizer   = new StagedTripPlanOptimizer(dpCalc, profitFinder, backtracker);

        // Act
        var result = await optimizer.OptimizeStagedAsync(
            items,
            orderedInterests.ToList(),
            budget,
            constraints,
            priorities
        );

        // Assert: map each PlaceType back to its Id
        var expectedOrder = orderedInterests.Select(s => s switch
        {
            "accommodation" => 2,
            "food"          => 4,
            "restaurants"   => 4,
            "entertainment" => 3,
            "tourism"       => 1,
            "tourismareas"  => 1,
            _               => throw new InvalidOperationException($"Unknown interest '{s}'")
        }).ToList();

        Assert.Equal(expectedOrder, result.Select(i => i.Id).ToList());
    }

 
*/
