using MediatR;
using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Restaurants.Commands.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Data.AppMetaData;
using TripMinder.Infrastructure.Data;

namespace TripMinder.API.Controllers
{
    [ApiController]
    public class RestaurantController : AppControllerBase
    {
        private readonly AppDBContext _ctx;
        
        public RestaurantController(AppDBContext ctx) => _ctx = ctx;
    
        [HttpGet(Router.AccomodationRouting.GetImage)]
        public IActionResult GetAccommodationImage(int id)
        {
            var img = _ctx.Restaurants
                .Where(r => r.Id == id)
                .Select(r => r.ImgData)
                .FirstOrDefault();
            if (img == null) return NotFound();
            // حدِّد الـ content-type بناءً على امتداد الصورة
            return File(img, "image/jpeg");
        }
        
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

        
        # region Filtering Endpoints
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
        
        [HttpGet(Router.RestaurantRouting.ListByClassId)]
        public async Task<IActionResult> GetRestaurantListByClassIdAsync([FromRoute]int classId, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListByClassIdQuery(classId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.RestaurantRouting.ListByFoodTypeId)]
        public async Task<IActionResult> GetRestaurantListByTypeIdAsync([FromRoute]int foodtypeId, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListByFoodTypeIdQuery(foodtypeId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.RestaurantRouting.ListByRating)]
        public async Task<IActionResult> GetRestaurantListByRatingAsync([FromRoute]float rating, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListByRatingQuery(rating, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.RestaurantRouting.ListMoreThanPrice)]
        public async Task<IActionResult> GetRestaurantListMoreThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListMoreThanPriceQuery(price, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.RestaurantRouting.ListLessThanPrice)]
        public async Task<IActionResult> GetRestaurantListLessThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
        {
            var query = new GetRestaurantsListLessThanPriceQuery(price, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        #endregion
        
        // [HttpPost(Router.RestaurantRouting.Create)]
        // public async Task<IActionResult> Create(CreateRestaurantCommand command)
        // {
        //     var response = await this.Mediator.Send(command);
        //     return NewResult(response);
        // }
        
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
