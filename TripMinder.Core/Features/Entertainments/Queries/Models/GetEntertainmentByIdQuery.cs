using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models
{

    public class GetEntertainmentByIdQuery : IRequest<Respond<GetEntertainmentByIdResponse>>
    {
        public int Id { get; set; }

        public GetEntertainmentByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}