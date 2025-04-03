using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models
{
    public class GetAccomodationsListQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
    {

    }
}
