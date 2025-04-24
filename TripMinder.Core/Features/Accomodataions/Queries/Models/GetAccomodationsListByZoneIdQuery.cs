using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByZoneIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int ZoneId { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByZoneIdQuery(int zoneId, int priority) => (this.ZoneId, this.Priority) = (zoneId, priority);
}