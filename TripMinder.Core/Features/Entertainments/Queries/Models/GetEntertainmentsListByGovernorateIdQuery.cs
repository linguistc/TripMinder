using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models;

public class GetEntertainmentsListByGovernorateIdQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
{
    public int GovernorateId { get; set; }
    public int Priority { get; set; }
        
    public GetEntertainmentsListByGovernorateIdQuery(int governorateId, int priority) => (this.GovernorateId, this.Priority) = (governorateId, priority);
}