using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByGovernorateIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int GovernorateId { get; set; }
    public int Priority { get; set; }
    
    public GetAccomodationsListByGovernorateIdQuery(int governorateId, int priority) => (this.GovernorateId, this.Priority) = (governorateId, priority); 
}