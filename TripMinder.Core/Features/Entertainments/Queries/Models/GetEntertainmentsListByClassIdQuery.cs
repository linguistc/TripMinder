using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models;

public class GetEntertainmentsListByClassIdQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
{
    public int ClassId { get; set; }
    public int Priority { get; set; }
        
    public GetEntertainmentsListByClassIdQuery(int classId, int priority) => (this.ClassId, this.Priority) = (classId, priority);
}