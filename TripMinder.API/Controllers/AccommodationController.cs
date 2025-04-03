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
            var response = await Mediator.Send(new GetAccomodationsListQuery());
            return Ok(response);
        }

        [HttpGet(Router.AccomodationRouting.GetById)]
        public async Task<IActionResult> GetAccommodationById(int id)
        {
            var response = await Mediator.Send(new GetAccomodationByIdQuery(id));
            return NewResult(response);
        }
    }
}
