using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models
{
    public class GetRestaurantsListQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
    {

    }
}
