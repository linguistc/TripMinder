using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Data.AppMetaData;
using System.ComponentModel.DataAnnotations;

namespace TripMinder.API.Controllers;

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
            interestsQueue
        );

        var response = await _optimizer.OptimizePlan(request);
        return response.Succeeded 
            ? Ok(response) 
            : BadRequest(new { response.Message, response.Errors });
    }
}

public record TripPlanRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ZoneId { get; init; }

    [Required]
    [Range(0, double.MaxValue)]
    public double BudgetPerAdult { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int NumberOfTravelers { get; init; }

    [Required]
    public List<string> Interests { get; init; }
}