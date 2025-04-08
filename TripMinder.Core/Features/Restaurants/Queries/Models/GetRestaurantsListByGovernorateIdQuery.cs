using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models;

public class GetRestaurantsListByGovernorateIdQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
{
    public int GovernorateId { get; set; }
    public int Priority { get; set; }
        
    public GetRestaurantsListByGovernorateIdQuery(int governorateId, int priority) => (this.GovernorateId, this.Priority) = (governorateId, priority);
}