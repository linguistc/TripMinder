using MediatR;
using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Restaurants.Commands.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers
{
    [ApiController]
    public class RestaurantController : AppControllerBase
    {
        [HttpGet(Router.RestaurantRouting.List)]
        public async Task<IActionResult> GetRestaurantListAsync()
        {
            var response = await Mediator.Send(new GetRestaurantsListQuery());
            return Ok(response);
        }

        [HttpGet(Router.RestaurantRouting.GetById)]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var response = await this.Mediator.Send(new GetRestaurantByIdQuery(id));
            return NewResult(response);
        }

        [HttpGet(Router.RestaurantRouting.ListByZoneId)]
        public async Task<IActionResult> GetRestaurantListByZoneIdAsync([FromRoute]int zoneId, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListByZoneIdQuery(zoneId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.RestaurantRouting.ListByGovernorateId)]
        public async Task<IActionResult> GetRestaurantListByGovernorateIdAsync([FromRoute]int governorateId, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListByGovernorateIdQuery(governorateId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpPost(Router.RestaurantRouting.Create)]
        public async Task<IActionResult> Create(CreateRestaurantCommand command)
        {
            var response = await this.Mediator.Send(command);
            return NewResult(response);
        }
        
        // [HttpPut(Router.RestaurantRouting.Update)]
        // public async Task<IActionResult> Update(UpdateRestaurantCommand command)
        // {
        //     var response = await this.Mediator.Send(command);
        //     return NewResult(response);
        // }
        //
        // [HttpDelete(Router.RestaurantRouting.Delete)]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var response = await this.Mediator.Send(new DeleteRestaurantCommand(id));
        //     return NewResult(response);
        // }

    }
}
