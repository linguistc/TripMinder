using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models;

public class GetTourismAreasListByTypeIdQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
{
    public int TypeId { get; set; }
    public int Priority { get; set; }
        
    public GetTourismAreasListByTypeIdQuery(int foodTypeId, int priority) => (this.TypeId, this.Priority) = (foodTypeId, priority);
}