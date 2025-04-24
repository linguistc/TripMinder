using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models;

public class GetRestaurantsListByRatingQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
{
    public double Rating { get; set; }
    public int Priority { get; set; }
        
    public GetRestaurantsListByRatingQuery(double rating, int priority) => (this.Rating, this.Priority) = (rating, priority);
}