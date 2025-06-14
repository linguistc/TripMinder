using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models;

public class GetRestaurantsListMoreThanPriceQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
{
    public decimal Price { get; set; }
    public int Priority { get; set; }
        
    public GetRestaurantsListMoreThanPriceQuery(decimal price, int priority) => (this.Price, this.Priority) = (price, priority);
}