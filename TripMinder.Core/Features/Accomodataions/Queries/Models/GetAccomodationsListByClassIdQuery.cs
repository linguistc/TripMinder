using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByClassIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int ClassId { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByClassIdQuery(int classId, int priority) => (this.ClassId, this.Priority) = (classId, priority);
}