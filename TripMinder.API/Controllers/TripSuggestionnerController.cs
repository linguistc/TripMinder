using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Data.AppMetaData;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging; // لو عايز Logging

namespace TripMinder.API.Controllers;

public record TripPlanRequestDto(
    [Required]
    [Range(1, int.MaxValue)]
    int ZoneId, 
    
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
    private readonly ILogger<TripSuggesterController> _logger; // إضافة Logger

    public TripSuggesterController(TripPlanOptimizer optimizer, ILogger<TripSuggesterController> logger)
    {
        this._optimizer = optimizer;
        this._logger = logger;
    }

    [HttpPost(Router.TripSuggesterRouting.OptimizeTrip)]
    public async Task<IActionResult> OptimizeTrip([FromBody] TripPlanRequestDto requestDto)
    {
        _logger.LogInformation("Received trip optimization request: {@Request}", requestDto);

        var interestsQueue = new Queue<string>(requestDto.Interests);
        var request = new TripPlanRequest(
            requestDto.ZoneId,
            requestDto.BudgetPerAdult,
            requestDto.NumberOfTravelers,
            interestsQueue,
            requestDto.MaxRestaurants,
            requestDto.MaxAccommodations,
            requestDto.MaxEntertainments,
            requestDto.MaxTourismAreas
        );

        var response = await _optimizer.OptimizePlan(request);

        if (response.Succeeded)
        {
            // تحقق إضافي لو عايز تتاكد إن الـ Response فيه بيانات كافية
            if (response.Data.Accommodation == null || 
                !response.Data.Restaurants.Any() || 
                !response.Data.Entertainments.Any() || 
                !response.Data.TourismAreas.Any())
            {
                _logger.LogWarning("Response is incomplete: {@Response}", response);
            }
            return Ok(response);
        }

        _logger.LogError("Optimization failed: {Message}, Errors: {@Errors}", response.Message, response.Errors);
        return BadRequest(new { response.Message, response.Errors });
    }
}