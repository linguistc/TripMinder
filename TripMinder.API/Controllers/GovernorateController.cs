using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Governorates.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class GovernorateController : AppControllerBase
{
    [HttpGet(Router.GovernorateRouting.List)]
    public async Task<IActionResult> GetGovernorateListAsync()
    {
        var response = await Mediator.Send(new GetGovernoratesListQuery());
        return Ok(response);
    }
    
    [HttpGet(Router.GovernorateRouting.GetById)]
    public async Task<IActionResult> GetGovernorateById(int id)
    {
        var response = await Mediator.Send(new GetGovernorateByIdQuery(id));
        return NewResult(response);
    }
}