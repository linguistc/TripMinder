using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByTypeIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int TypeId { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByTypeIdQuery(int typeId, int priority) => (this.TypeId, this.Priority) = (typeId, priority);
}