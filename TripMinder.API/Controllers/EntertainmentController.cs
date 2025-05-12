using MediatR;
using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Data.AppMetaData;
using TripMinder.Infrastructure.Data;

namespace TripMinder.API.Controllers;

[ApiController]
public class EntertainmentController : AppControllerBase
{
    private readonly AppDBContext _ctx;
        
    public EntertainmentController(AppDBContext ctx) => _ctx = ctx;
    
    [HttpGet(Router.AccomodationRouting.GetImage)]
    public IActionResult GetAccommodationImage(int id)
    {
        var img = _ctx.Entertainments
            .Where(r => r.Id == id)
            .Select(r => r.ImgData)
            .FirstOrDefault();
        if (img == null) return NotFound();
        // حدِّد الـ content-type بناءً على امتداد الصورة
        return File(img, "image/jpeg");
    }

        
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

    #region Filtering Endpoints
    
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
    
    [HttpGet(Router.EntertainmentRouting.ListByClassId)]
    public async Task<IActionResult> GetEntertainmentListByClassIdAsync([FromRoute]int classId, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListByClassIdQuery(classId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.EntertainmentRouting.ListByTypeId)]
    public async Task<IActionResult> GetEntertainmentListByTypeIdAsync([FromRoute]int typeId, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListByTypeIdQuery(typeId, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.EntertainmentRouting.ListByRating)]
    public async Task<IActionResult> GetEntertainmentListByRatingAsync([FromRoute]float rating, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListByRatingQuery(rating, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.EntertainmentRouting.ListMoreThanPrice)]
    public async Task<IActionResult> GetEntertainmentListMoreThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListMoreThanPriceQuery(price, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet(Router.EntertainmentRouting.ListLessThanPrice)]
    public async Task<IActionResult> GetEntertainmentListLessThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
    {
        var query = new GetEntertainmentsListLessThanPriceQuery(price, priority ?? 1);
        var response = await this.Mediator.Send(query);
        return Ok(response);
    }
    #endregion
    
}
