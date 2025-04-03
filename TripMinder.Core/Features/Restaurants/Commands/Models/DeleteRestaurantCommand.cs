using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Restaurants.Commands.Models
{
    public class DeleteRestaurantCommand : IRequest<Respond<string>>
    {
        public int Id { get; set; }

        public DeleteRestaurantCommand(int id)
        {
            this.Id = id;
        }
    }
}
