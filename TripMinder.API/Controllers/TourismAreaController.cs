using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class TourismAreaController : AppControllerBase
{
        
    [HttpGet(Router.TourismAreaRouting.List)]
    public async Task<IActionResult> GetTourismAreaListAsync()
    {
        var response = await this.Mediator.Send(new GetTourismAreasListQuery());
        return Ok(response);
    }
        
    [HttpGet(Router.TourismAreaRouting.GetById)]
    public async Task<IActionResult> GetTourismAreaById(int id)
    {
        var response = await this.Mediator.Send(new GetTourismAreaByIdQuery(id));

        return NewResult(response);
    }

    #region Filtering Endpoints
    [HttpGet(Router.TourismAreaRouting.ListByZoneId)]
    public async Task<IActionResult> GetTourismAreaListByZoneIdAsync([FromRoute]int zoneId, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListByZoneIdQuery(zoneId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.TourismAreaRouting.ListByGovernorateId)]
    public async Task<IActionResult> GetTourismAreaListByGovernorateIdAsync([FromRoute]int governorateId, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListByGovernorateIdQuery(governorateId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.TourismAreaRouting.ListByClassId)]
    public async Task<IActionResult> GetTourismAreaListByClassIdAsync([FromRoute]int classId, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListByClassIdQuery(classId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    
    [HttpGet(Router.TourismAreaRouting.ListByTypeId)]
    public async Task<IActionResult> GetTourismAreaListByTypeIdAsync([FromRoute]int typeId, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListByTypeIdQuery(typeId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.TourismAreaRouting.ListByRating)]
    public async Task<IActionResult> GetTourismAreaListByRatingAsync([FromRoute]float rating, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListByRatingQuery(rating, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.TourismAreaRouting.ListLessThanPrice)]
    public async Task<IActionResult> GetTourismAreaListLessThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListLessThanPriceQuery(price, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    
    [HttpGet(Router.TourismAreaRouting.ListMoreThanPrice)]
    public async Task<IActionResult> GetTourismAreaListMoreThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
    {
        var query = new GetTourismAreasListMoreThanPriceQuery(price, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    #endregion
}