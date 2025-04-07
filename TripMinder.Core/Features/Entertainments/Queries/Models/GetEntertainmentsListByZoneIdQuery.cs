using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models;

public class GetEntertainmentsListByZoneIdQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
{
    public int ZoneId { get; set; }
    public int Priority { get; set; }
        
    public GetEntertainmentsListByZoneIdQuery(int zoneId, int priority) => (this.ZoneId, this.Priority) = (zoneId, priority);
}