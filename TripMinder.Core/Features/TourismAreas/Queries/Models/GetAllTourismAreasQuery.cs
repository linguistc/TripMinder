using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

namespace TripMinder.Core.Features.TourismAreas.Queries.Models
{
    public class GetAllTourismAreasQuery : IRequest<Respond<List<GetTourismAreasListResponse>>>
    {
    }
}