using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models
{
    public class GetTourismAreasListQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
    {
    }
}