using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByNumberOfAdultsQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public short NumOfAdults { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByNumberOfAdultsQuery(short numOfAdults, int priority) => (this.NumOfAdults, this.Priority) = (numOfAdults, priority);
}