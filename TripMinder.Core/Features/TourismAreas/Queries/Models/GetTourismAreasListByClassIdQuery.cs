using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models;

public class GetTourismAreasListByClassIdQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
{
    public int ClassId { get; set; }
    public int Priority { get; set; }
        
    public GetTourismAreasListByClassIdQuery(int classId, int priority) => (this.ClassId, this.Priority) = (classId, priority);
}