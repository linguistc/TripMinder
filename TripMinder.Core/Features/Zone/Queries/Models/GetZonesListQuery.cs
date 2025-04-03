using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Zone.Queries.Responses;

namespace TripMinder.Core.Features.Zone.Queries.Models;

public class GetZonesListQuery : IRequest<Respond<List<GetZonesListResponse>>>
{
    
}