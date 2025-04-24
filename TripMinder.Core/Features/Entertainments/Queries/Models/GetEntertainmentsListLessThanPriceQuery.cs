using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models;

public class GetEntertainmentsListLessThanPriceQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
{
    public decimal Price { get; set; }
    public int Priority { get; set; }
        
    public GetEntertainmentsListLessThanPriceQuery(decimal price, int priority) => (this.Price, this.Priority) = (price, priority);
}