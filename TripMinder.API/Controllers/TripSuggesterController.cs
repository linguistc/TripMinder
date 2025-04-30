using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Data.AppMetaData;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace TripMinder.API.Controllers;

public record TripPlanRequestDto(
    [Required]
    [Range(1, int.MaxValue)]
    int GovernorateId,

    [Range(1, int.MaxValue)]
    int? ZoneId,
    
    [Required]
    [Range(0, double.MaxValue)]
    double BudgetPerAdult,
    
    [Required]
    [Range(1, int.MaxValue)]
    int NumberOfTravelers,
    
    [Required]
    List<string> Interests, 
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "MaxRestaurants must be non-negative")]
    int MaxRestaurants, 
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "MaxAccommodations must be non-negative")]
    int MaxAccommodations, 
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "MaxEntertainments must be non-negative")]
    int MaxEntertainments, 
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "MaxTourismAreas must be non-negative")]
    int MaxTourismAreas);

[ApiController]
public class TripSuggesterController : AppControllerBase
{
    private readonly TripPlanOptimizer _optimizer;
    private readonly ILogger<TripSuggesterController> _logger;

    public TripSuggesterController(TripPlanOptimizer optimizer, ILogger<TripSuggesterController> logger)
    {
        this._optimizer = optimizer;
        this._logger = logger;
    }

    [HttpPost(Router.TripSuggesterRouting.OptimizeTrip)]
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