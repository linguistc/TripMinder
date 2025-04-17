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

        var interestsQueue = new Queue<string>(requestDto.Interests);
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

        if (response.Succeeded)
        {
            _logger.LogInformation("Optimization succeeded with {Count} solutions", response.Data.Count);
            return Ok(response);
        }

        _logger.LogError("Optimization failed: {Message}, Errors: {@Errors}", response.Message, response.Errors);
        return BadRequest(new Respond<List<TripPlanResponse>>
        {
            Succeeded = false,
            Message = response.Message,
            Errors = response.Errors,
            ErrorsBag = new Dictionary<string, List<string>> { { "General", response.Errors.ToList() } }
        });
    }
}