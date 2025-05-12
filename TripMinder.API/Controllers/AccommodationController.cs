using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Data.AppMetaData;
using TripMinder.Infrastructure.Data;

namespace TripMinder.API.Controllers
{
    [ApiController]
    public class AccommodationController : AppControllerBase
    {
        private readonly AppDBContext _ctx;
        
        public AccommodationController(AppDBContext ctx) => _ctx = ctx;
        
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

        [HttpGet(Router.AccomodationRouting.GetImage)]
        public IActionResult GetAccommodationImage(int id)
        {
            var img = _ctx.Accomodations
                .Where(r => r.Id == id)
                .Select(r => r.ImgData)
                .FirstOrDefault();
            if (img == null) return NotFound();
            // حدِّد الـ content-type بناءً على امتداد الصورة
            return File(img, "image/jpeg");
        }

        #region Filtering Endpoints
        
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
        
        [HttpGet(Router.AccomodationRouting.ListByTypeId)]
        public async Task<IActionResult> GetAccommodationListByTypeIdAsync([FromRoute]int typeId, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListByTypeIdQuery(typeId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.AccomodationRouting.ListByClassId)]
        public async Task<IActionResult> GetAccommodationListByClassIdAsync([FromRoute]int classId, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListByClassIdQuery(classId, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.AccomodationRouting.ListByRating)]
        public async Task<IActionResult> GetAccommodationListByRatingAsync([FromRoute]float rating, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListByRatingQuery(rating, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.AccomodationRouting.ListLessThanPrice)]
        public async Task<IActionResult> GetAccommodationListLessThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListLessThanPriceQuery(price, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.AccomodationRouting.ListMoreThanPrice)]
        public async Task<IActionResult> GetAccommodationListMoreThanPriceAsync([FromRoute]decimal price, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListMoreThanPriceQuery(price, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        
        [HttpGet(Router.AccomodationRouting.ListByNumOfAdults)]
        public async Task<IActionResult> GetAccommodationListByNumOfAdultsAsync([FromRoute]short numOfAdults, [FromQuery]int? priority = 1)
        {
            var query = new GetAccomodationsListByNumberOfAdultsQuery(numOfAdults, priority ?? 1);
            var response = await this.Mediator.Send(query);
            return Ok(response);
        }
        #endregion
    }
}

