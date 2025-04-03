using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Zone.Commands.Models;
using TripMinder.Core.Features.Zone.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class ZoneController : AppControllerBase
{
    [HttpGet(Router.ZoneRouting.List)]
    public async Task<IActionResult> GetZoneListAsync()
    {
        var response = await Mediator.Send(new GetZonesListQuery());
        return Ok(response);
    }
    
    [HttpGet(Router.ZoneRouting.GetById)]
    public async Task<IActionResult> GetZoneById(int id)
    {
        var response = await Mediator.Send(new GetZoneByIdQuery(id));
        return NewResult(response);
    }
    
    [HttpPost(Router.ZoneRouting.Create)]
    public async Task<IActionResult> Create(CreateZoneCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }
    
    [HttpPut(Router.ZoneRouting.Update)]
    public async Task<IActionResult> Update(UpdateZoneCommand command)
    {
        var response = await Mediator.Send(command);
        return NewResult(response);
    }
    
    [HttpDelete(Router.ZoneRouting.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await Mediator.Send(new DeleteZoneCommand(id));
        return NewResult(response);
    }
}