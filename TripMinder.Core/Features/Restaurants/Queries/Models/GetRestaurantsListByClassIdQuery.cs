using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models;

public class GetRestaurantsListByClassIdQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
{
    public int ClassId { get; set; }
    public int Priority { get; set; }
        
    public GetRestaurantsListByClassIdQuery(int classId, int priority) => (this.ClassId, this.Priority) = (classId, priority);
}