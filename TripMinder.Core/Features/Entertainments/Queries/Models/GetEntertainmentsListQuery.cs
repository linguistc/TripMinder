using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models
{

    public class GetEntertainmentsListQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
    {
    
    }
}