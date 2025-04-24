using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByRatingQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public double Rating { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByRatingQuery(double rating, int priority) => (this.Rating, this.Priority) = (rating, priority);
}