using System.Net;
using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models
{
    public class GetAllRestaurantsQuery : IRequest<Respond<List<GetAllRestaurantsResponse>>>
    {

    }
}
