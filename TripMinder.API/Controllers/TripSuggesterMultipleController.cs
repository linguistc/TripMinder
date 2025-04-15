using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class TripSuggesterMultipleController : AppControllerBase
{
    private readonly TripPlanOptimizer _optimizer;
    private readonly ILogger<TripSuggesterMultipleController> _logger; // إضافة Logger

    public TripSuggesterMultipleController(TripPlanOptimizer optimizer, ILogger<TripSuggesterMultipleController> logger)
    {
        this._optimizer = optimizer;
        this._logger = logger;
    }
    
    [HttpPost(Router.TripSuggesterMultipleRouting.OptimizeTrip)]
    public async Task<IActionResult> OptimizeMultipleTrips([FromBody] TripPlanRequestDto requestDto)
    {
        _logger.LogInformation("Received multiple trip optimization request: {@Request}", requestDto);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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
        return BadRequest(new { response.Message, response.Errors });
    }
}