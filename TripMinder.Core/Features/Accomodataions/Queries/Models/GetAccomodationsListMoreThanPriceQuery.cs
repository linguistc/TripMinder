using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListMoreThanPriceQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public decimal Price { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListMoreThanPriceQuery(decimal price, int priority) => (this.Price, this.Priority) = (price, priority);
}