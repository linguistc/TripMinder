using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Restaurants.Queries.Responses;

namespace TripMinder.Core.Features.Restaurants.Queries.Models
{
    public class GetRestaurantByIdQuery : IRequest<Response<GetSingleRestaurantResponse>>
    {
        public int Id { get; set; }

        public GetRestaurantByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
