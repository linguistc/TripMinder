using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Data.AppMetaData;
using System.ComponentModel.DataAnnotations;

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

    public TripSuggesterController(TripPlanOptimizer optimizer)
    {
        this._optimizer = optimizer;
    }

    [HttpPost(Router.TripSuggestionnerRouting.OptimizeTrip)]
    public async Task<IActionResult> OptimizeTrip([FromBody] TripPlanRequestDto requestDto)
    {
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
        return response.Succeeded 
            ? Ok(response) 
            : BadRequest(new { response.Message, response.Errors });
    }
}
