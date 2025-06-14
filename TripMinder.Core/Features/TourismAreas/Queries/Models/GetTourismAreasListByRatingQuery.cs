using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models;

public class GetTourismAreasListByRatingQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
{
    public double Rating { get; set; }
    public int Priority { get; set; }
        
    public GetTourismAreasListByRatingQuery(double rating, int priority) => (this.Rating, this.Priority) = (rating, priority);
}