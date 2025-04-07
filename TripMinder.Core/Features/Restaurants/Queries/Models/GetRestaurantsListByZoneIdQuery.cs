using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models;

public class GetRestaurantsListByZoneIdQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
{
    public int ZoneId { get; set; }
    public int Priority { get; set; }
        
    public GetRestaurantsListByZoneIdQuery(int zoneId, int priority) => (this.ZoneId, this.Priority) = (zoneId, priority);
}