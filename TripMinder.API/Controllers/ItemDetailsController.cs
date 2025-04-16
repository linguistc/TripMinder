using MediatR;
using Microsoft.AspNetCore.Mvc;
using TripMinder.API.Bases;
using TripMinder.Core.Behaviors.Knapsack;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Restaurants.Queries.Models;
using TripMinder.Data.AppMetaData;

namespace TripMinder.API.Controllers;

[ApiController]
public class ItemDetailsController : AppControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ItemDetailsController> _logger;

    public ItemDetailsController(IMediator mediator, ILogger<ItemDetailsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets the details of an item by its ID and PlaceType.
    /// </summary>
    /// <param name="id">The ID of the item.</param>
    /// <param name="placeType">The type of place (Accommodation, Restaurant, Entertainment, TourismArea).</param>
    /// <returns>
    /// Returns the item details specific to the PlaceType:
    /// - Accommodation: Includes NumOfBeds, BedStatus, NumOfMembers, AccomodationType.
    /// - Restaurant: Includes FoodCategory.
    /// - Entertainment: Includes EntertainmentType.
    /// - TourismArea: Includes TourismType.
    /// All responses include common fields: Id, Name, AveragePricePerAdult, Score, Rating, ImageSource, ZoneId, ZoneName, GovernorateId, GovernorateName, Description, Address, MapLink, HasKidsArea, ContactLink, ClassType.
    /// </returns>
    [HttpGet(Router.ItemDetailsRouting.GetItemByIdAndPlaceType)]
    public async Task<IActionResult> GetItemDetails([FromRoute] int id, [FromQuery] ItemType placeType)
    {
        _logger.LogInformation("Received item details request: ItemId={ItemId}, PlaceType={PlaceType}", id, placeType);

        switch (placeType)
        {
            case ItemType.Accommodation:
                var accomodationResponse = await _mediator.Send(new GetAccomodationByIdQuery(id));
                if (accomodationResponse == null)
                {
                    _logger.LogWarning("Accommodation not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(accomodationResponse);

            case ItemType.Restaurant:
                var restaurantResponse = await _mediator.Send(new GetRestaurantByIdQuery(id));
                if (restaurantResponse == null)
                {
                    _logger.LogWarning("Restaurant not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(restaurantResponse);

            case ItemType.Entertainment:
                var entertainmentResponse = await _mediator.Send(new GetEntertainmentByIdQuery(id));
                if (entertainmentResponse == null)
                {
                    _logger.LogWarning("Entertainment not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(entertainmentResponse);

            case ItemType.TourismArea:
                var tourismAreaResponse = await _mediator.Send(new GetTourismAreaByIdQuery(id));
                if (tourismAreaResponse == null)
                {
                    _logger.LogWarning("TourismArea not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(tourismAreaResponse);

            default:
                _logger.LogWarning("Invalid PlaceType: {PlaceType}", placeType);
                return BadRequest(new { Error = "Invalid PlaceType" });
        }
    }
}



/*

[ApiController]
public class ItemDetailsController : AppControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ItemDetailsController> _logger;

    public ItemDetailsController(IMediator mediator, ILogger<ItemDetailsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public record ItemDetailsResponse(
        int Id,
        string Name,
        double AveragePricePerAdult,
        float Score,
        ItemType PlaceType,
        string ClassType,
        double Rating,
        string ImageSource,
        int ZoneId,
        string ZoneName,
        int GovernorateId,
        string GovernorateName,
        string Description,
        string Address,
        string MapLink,
        bool HasKidsArea,
        string ContactLink,
        int CategoryId,
        string CategoryName,
        int? NumOfBeds,
        string BedStatus,
        int? NumOfMembers
    );

    [HttpGet(Router.ItemDetailsRouting.GetItemByIdAndPlaceType)]
    public async Task<IActionResult> GetItemDetails([FromRoute] int id, [FromQuery] ItemType placeType)
    {
        _logger.LogInformation("Received item details request: ItemId={ItemId}, PlaceType={PlaceType}", id, placeType);

        switch (placeType)
        {
            case ItemType.Accommodation:
                var accomodationResponse = await _mediator.Send(new GetAccomodationByIdQuery(id));
                if (accomodationResponse == null)
                {
                    _logger.LogWarning("Accommodation not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(new ItemDetailsResponse(
                    accomodationResponse.Id,
                    accomodationResponse.Name,
                    accomodationResponse.AveragePricePerAdult,
                    accomodationResponse.Score,
                    ItemType.Accommodation,
                    accomodationResponse.ClassType,
                    accomodationResponse.Rating,
                    accomodationResponse.ImageSource,
                    accomodationResponse.ZoneId,
                    accomodationResponse.ZoneName,
                    accomodationResponse.GovernorateId,
                    accomodationResponse.GovernorateName,
                    accomodationResponse.Description,
                    accomodationResponse.Address,
                    accomodationResponse.MapLink,
                    accomodationResponse.HasKidsArea,
                    accomodationResponse.ContactLink,
                    accomodationResponse.AccomodationTypeId,
                    accomodationResponse.AccomodationTypeName,
                    accomodationResponse.NumOfBeds,
                    accomodationResponse.BedStatus,
                    accomodationResponse.NumOfMembers
                ));

            case ItemType.Restaurant:
                var restaurantResponse = await _mediator.Send(new GetRestaurantByIdQuery(id));
                if (restaurantResponse == null)
                {
                    _logger.LogWarning("Restaurant not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(new ItemDetailsResponse(
                    restaurantResponse.Id,
                    restaurantResponse.Name,
                    restaurantResponse.AveragePricePerAdult,
                    restaurantResponse.Score,
                    ItemType.Restaurant,
                    restaurantResponse.ClassType,
                    restaurantResponse.Rating,
                    restaurantResponse.ImageSource,
                    restaurantResponse.ZoneId,
                    restaurantResponse.ZoneName,
                    restaurantResponse.GovernorateId,
                    restaurantResponse.GovernorateName,
                    restaurantResponse.Description,
                    restaurantResponse.Address,
                    restaurantResponse.MapLink,
                    restaurantResponse.HasKidsArea,
                    restaurantResponse.ContactLink,
                    restaurantResponse.FoodCategoryId,
                    restaurantResponse.FoodCategoryName,
                    null,
                    null,
                    null
                ));

            case ItemType.Entertainment:
                var entertainmentResponse = await _mediator.Send(new GetEntertainmentByIdQuery(id));
                if (entertainmentResponse == null)
                {
                    _logger.LogWarning("Entertainment not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(new ItemDetailsResponse(
                    entertainmentResponse.Id,
                    entertainmentResponse.Name,
                    entertainmentResponse.AveragePricePerAdult,
                    entertainmentResponse.Score,
                    ItemType.Entertainment,
                    entertainmentResponse.ClassType,
                    entertainmentResponse.Rating,
                    entertainmentResponse.ImageSource,
                    entertainmentResponse.ZoneId,
                    entertainmentResponse.ZoneName,
                    entertainmentResponse.GovernorateId,
                    entertainmentResponse.GovernorateName,
                    entertainmentResponse.Description,
                    entertainmentResponse.Address,
                    entertainmentResponse.MapLink,
                    entertainmentResponse.HasKidsArea,
                    entertainmentResponse.ContactLink,
                    entertainmentResponse.EntertainmentTypeId,
                    entertainmentResponse.EntertainmentTypeName,
                    null,
                    null,
                    null
                ));

            case ItemType.TourismArea:
                var tourismAreaResponse = await _mediator.Send(new GetTourismAreaByIdQuery(id));
                if (tourismAreaResponse == null)
                {
                    _logger.LogWarning("TourismArea not found: ItemId={ItemId}", id);
                    return NotFound();
                }
                return Ok(new ItemDetailsResponse(
                    tourismAreaResponse.Id,
                    tourismAreaResponse.Name,
                    tourismAreaResponse.AveragePricePerAdult,
                    tourismAreaResponse.Score,
                    ItemType.TourismArea,
                    tourismAreaResponse.ClassType,
                    tourismAreaResponse.Rating,
                    tourismAreaResponse.ImageSource,
                    tourismAreaResponse.ZoneId,
                    tourismAreaResponse.ZoneName,
                    tourismAreaResponse.GovernorateId,
                    tourismAreaResponse.GovernorateName,
                    tourismAreaResponse.Description,
                    tourismAreaResponse.Address,
                    tourismAreaResponse.MapLink,
                    tourismAreaResponse.HasKidsArea,
                    tourismAreaResponse.ContactLink,
                    tourismAreaResponse.TourismTypeId,
                    tourismAreaResponse.TourismTypeName,
                    null,
                    null,
                    null
                ));

            default:
                _logger.LogWarning("Invalid PlaceType: {PlaceType}", placeType);
                return BadRequest(new { Error = "Invalid PlaceType" });
        }
    }
}

*/

/*
 * Frontend code
 *
 Future<void> getItemDetails(int itemId, String placeType) async {
  final response = await http.get(Uri.parse('https://your-api/api/item/$itemId?placeType=$placeType'));
  if (response.statusCode == 200) {
    final json = jsonDecode(response.body);
    print('Name: ${json['name']}');
    if (placeType == 'Accommodation') {
      print('Beds: ${json['numOfBeds']}');
      print('Bed Status: ${json['bedStatus']}');
    } else if (placeType == 'Restaurant') {
      print('Food Category: ${json['foodCategoryId']}');
    }
  } else {
    throw Exception('Failed to load item details');
  }
}
 */