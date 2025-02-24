using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models
{

    public class GetAllEntertainmentsQuery : IRequest<Respond<List<GetAllEntertainmentsResponse>>>
    {
    
    }

}