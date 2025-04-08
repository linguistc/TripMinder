using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models;

public class GetTourismAreasListByZoneIdQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
{
    public int ZoneId { get; set; }
    public int Priority { get; set; }
        
    public GetTourismAreasListByZoneIdQuery(int zoneId, int priority) => (this.ZoneId, this.Priority) = (zoneId, priority);
}