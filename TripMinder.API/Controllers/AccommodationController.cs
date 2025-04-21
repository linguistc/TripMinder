using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers
{
    [ApiController]
    public class AccommodationController : AppControllerBase
    {
        [HttpGet(Router.AccomodationRouting.List)]
        public async Task<IActionResult> GetAccommodationListAsync()
        {
            var response = await this.Mediator.Send(new GetAccomodationsListQuery());
            return Ok(response);
        }
        
        [HttpGet(Router.AccomodationRouting.GetById)]
        public async Task<IActionResult> GetAccommodationById(int id)
        {
            var response = await this.Mediator.Send(new GetAccomodationByIdQuery(id));
            return NewResult(response);
        }
        
        [HttpGet(Router.AccomodationRouting.ListByZoneId)]
        public async Task<IActionResult> GetAccommodationListByZoneIdAsync([FromRoute]int zoneId, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListByZoneIdQuery(zoneId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }

        
        [HttpGet(Router.AccomodationRouting.ListByGovernorateId)]
        public async Task<IActionResult> GetAccommodationListByGovernorateIdAsync([FromRoute]int governorateId, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListByGovernorateIdQuery(governorateId, priority ?? 1);
            
            var response = await this.Mediator.Send(query);

            return Ok(response);
        }
    }
}
