using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByNumberOfBedsQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public short NumOfBeds { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByNumberOfBedsQuery(short numOfBeds, int priority) => (this.NumOfBeds, this.Priority) = (numOfBeds, priority);
}