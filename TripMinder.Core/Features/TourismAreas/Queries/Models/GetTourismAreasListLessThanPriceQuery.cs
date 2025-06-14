using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models;

public class GetTourismAreasListLessThanPriceQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
{
    public decimal Price { get; set; }
    public int Priority { get; set; }
        
    public GetTourismAreasListLessThanPriceQuery(decimal price, int priority) => (this.Price, this.Priority) = (price, priority);
}