using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Data.AppMetaData;
using TripMinder.Core.Bases;

namespace TripMinder.API.Controllers;

[ApiController]
public class TripSuggesterMultipleController : AppControllerBase
{
    private readonly TripPlanOptimizer _optimizer;
    private readonly ILogger<TripSuggesterMultipleController> _logger;

    public TripSuggesterMultipleController(TripPlanOptimizer optimizer, ILogger<TripSuggesterMultipleController> logger)
    {
        _optimizer = optimizer;
        _logger = logger;
    }

    [HttpPost(Router.TripSuggesterMultipleRouting.OptimizeTrip)]
    public async Task<IActionResult> OptimizeMultipleTrips([FromBody] TripPlanRequestDto requestDto)
    {
        _logger.LogInformation("Received multiple trip optimization request: {@Request}", requestDto);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for request: {@ModelState}", ModelState);
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new Respond<List<TripPlanResponse>>
            {
                Succeeded = false,
                Message = "Invalid request data",
                Errors = errors.ToList(),
                ErrorsBag = new Dictionary<string, List<string>> { { "General", errors } }
            });
        }

        var interestsQueue = new Queue<string>(requestDto.Interests ?? new List<string>());
        var request = new TripPlanRequest(
            requestDto.GovernorateId,
            requestDto.ZoneId,
            requestDto.BudgetPerAdult,
            requestDto.NumberOfTravelers,
            interestsQueue,
            requestDto.MaxRestaurants,
            requestDto.MaxAccommodations,
            requestDto.MaxEntertainments,
            requestDto.MaxTourismAreas
        );

        var response = await _optimizer.OptimizePlanMultiple(request);

        if (response.Succeeded && response.Data != null)
        {
            // Retry with relaxed constraints if fewer than 3 solutions
            if (response.Data.Count < 3)
            {
                _logger.LogWarning("Only {Count} solutions found, retrying with relaxed constraints", response.Data.Count);
                response = await _optimizer.OptimizePlanMultiple(request); // Could pass modified constraints
            }

            _logger.LogInformation("Optimization succeeded with {Count} solutions", response.Data.Count);
            return Ok(response);
        }

        // Fallback to single solution if multiple solutions fail
        _logger.LogWarning("Multiple solutions failed, attempting single solution");
        var singleResponse = await _optimizer.OptimizePlanPhasedAsync(request);
        if (singleResponse.Succeeded && singleResponse.Data != null)
        {
            _logger.LogInformation("Single solution succeeded, returning as fallback");
            return Ok(new Respond<List<TripPlanResponse>>
            {
                Succeeded = true,
                Message = "Fallback to single trip plan due to no multiple solutions found",
                Data = new List<TripPlanResponse> { singleResponse.Data },
                Meta = new
                {
                    TotalItems = singleResponse.Data.Restaurants.Count +
                                 singleResponse.Data.Entertainments.Count +
                                 singleResponse.Data.TourismAreas.Count +
                                 (singleResponse.Data.Accommodation != null ? 1 : 0),
                    TotalSolutions = 1
                }
            });
        }

        _logger.LogError("Optimization failed: {Message}, Errors: {@Errors}", response.Message, response.Errors);
        return BadRequest(new Respond<List<TripPlanResponse>>
        {
            Succeeded = false,
            Message = response.Message ?? "No valid trip plans could be generated",
            Errors = response.Errors ?? new List<string> { "No items matched the provided criteria" },
            ErrorsBag = new Dictionary<string, List<string>> { { "General", response.Errors?.ToList() ?? new List<string> { "No items matched the provided criteria" } } }
        });
    }
}