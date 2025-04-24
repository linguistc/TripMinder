using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Responses;

namespace TripMinder.Core.Features.Entertainments.Queries.Models;

public class GetEntertainmentsListByRatingQuery : IRequest<Respond<List<GetEntertainmentsListResponse>>>
{
    public double Rating { get; set; }
    public int Priority { get; set; }
        
    public GetEntertainmentsListByRatingQuery(double rating, int priority) => (this.Rating, this.Priority) = (rating, priority);
}