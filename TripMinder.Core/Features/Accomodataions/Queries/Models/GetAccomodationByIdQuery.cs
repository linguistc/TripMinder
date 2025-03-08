using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models
{
    public class GetAccomodationByIdQuery : IRequest<Respond<GetAccomodationByIdResponse>>
    {
        public int Id { get; set; }

        public GetAccomodationByIdQuery(int id)
        {
            this.Id = id;
        }
    }
}
