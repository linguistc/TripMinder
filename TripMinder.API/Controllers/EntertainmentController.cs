using MediatR;
using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class EntertainmentController : AppControllerBase
{
        
    [HttpGet(Router.EntertainmentRouting.List)]
    public async Task<IActionResult> GetEntertainmentListAsync()
    {
        var response = await this.Mediator.Send(new GetEntertainmentsListQuery());
            
        return Ok(response);
    }
        
    [HttpGet(Router.EntertainmentRouting.GetById)]
    public async Task<IActionResult> GetEntertainmentById(int id)
    {
        var response = await this.Mediator.Send(new GetEntertainmentByIdQuery(id));
        return NewResult(response);
    }
    
    [HttpGet(Router.EntertainmentRouting.ListByZoneId)]
    public async Task<IActionResult> GetEntertainmentListByZoneIdAsync([FromRoute]int zoneId, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListByZoneIdQuery(zoneId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.EntertainmentRouting.ListByGovernorateId)]
    public async Task<IActionResult> GetEntertainmentListByGovernorateIdAsync([FromRoute]int governorateId, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListByGovernorateIdQuery(governorateId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }   
}