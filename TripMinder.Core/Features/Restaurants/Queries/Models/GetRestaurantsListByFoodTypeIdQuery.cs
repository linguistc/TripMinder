using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models;

public class GetRestaurantsListByFoodTypeIdQuery : IRequest<Respond<List<GetRestaurantsListResponse>>>
{
    public int FoodTypeId { get; set; }
    public int Priority { get; set; }
        
    public GetRestaurantsListByFoodTypeIdQuery(int foodTypeId, int priority) => (this.FoodTypeId, this.Priority) = (foodTypeId, priority);
}