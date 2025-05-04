using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Behaviors.Shared;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class DPTripSuggesterController : AppControllerBase
{
    private readonly TripPlanOptimizer _optimizer;
    private readonly ILogger<TripSuggesterController> _logger;

    public DPTripSuggesterController(TripPlanOptimizer optimizer, ILogger<TripSuggesterController> logger)
    {
        this._optimizer = optimizer;
        this._logger = logger;
    }

    [HttpPost(Router.DPTripSuggesterRouting.DPOptimizeTrip)]
    public async Task<IActionResult> OptimizeTrip([FromBody] TripPlanRequestDto requestDto)
    {
        _logger.LogInformation("Received trip optimization request: GovernorateId={GovernorateId}, ZoneId={ZoneId}, BudgetPerAdult={BudgetPerAdult}, NumberOfTravelers={NumberOfTravelers}, MaxRestaurants={MaxRestaurants}, Interests={Interests}",
            requestDto.GovernorateId, requestDto.ZoneId, requestDto.BudgetPerAdult, requestDto.NumberOfTravelers, requestDto.MaxRestaurants, string.Join(", ", requestDto.Interests));

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
            return BadRequest(ModelState);
        }
        
        var totalBudget = (int)(requestDto.BudgetPerAdult * requestDto.NumberOfTravelers);
        _logger.LogInformation("Calculated Total Budget: {TotalBudget}", totalBudget);

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

        var response = await _optimizer.OptimizePlanPhasedAsync(request);

        if (response.Succeeded)
        {
            if (response.Data.Accommodation == null || 
                !response.Data.Restaurants.Any() || 
                !response.Data.Entertainments.Any() || 
                !response.Data.TourismAreas.Any())
            {
                _logger.LogWarning("Response is incomplete: {@Response}", response);
            }
            _logger.LogInformation("Optimization succeeded: Restaurants={RestaurantsCount}, TotalCost={TotalCost}",
                response.Data.Restaurants.Count, response.Data.Restaurants.Sum(r => r.AveragePricePerAdult));
            return Ok(response);
        }

        _logger.LogError("Optimization failed: {Message}, Errors: {@Errors}", response.Message, response.Errors);
        return BadRequest(new { response.Message, response.Errors });
    }
}