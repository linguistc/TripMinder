using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models;

public class GetEntertainmentsListByTypeIdQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
{
    public int TypeId { get; set; }
    public int Priority { get; set; }
        
    public GetEntertainmentsListByTypeIdQuery(int typeId, int priority) => (this.TypeId, this.Priority) = (typeId, priority);
}