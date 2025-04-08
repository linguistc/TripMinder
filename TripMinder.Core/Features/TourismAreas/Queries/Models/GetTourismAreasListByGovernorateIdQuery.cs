using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models;

public class GetTourismAreasListByGovernorateIdQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
{
    public int GovernorateId { get; set; }
    public int Priority { get; set; }
        
    public GetTourismAreasListByGovernorateIdQuery(int governorateId, int priority) => (this.GovernorateId, this.Priority) = (governorateId, priority);
}