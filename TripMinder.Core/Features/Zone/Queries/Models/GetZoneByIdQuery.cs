using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Zone.Queries.Responses;

namespace TripMinder.Core.Features.Zone.Queries.Models;

public class GetZoneByIdQuery : IRequest<Respond<GetZoneByIdResponse>>
{
    public int Id { get; set; }

    public GetZoneByIdQuery(int id)
    {
        this.Id = id;
    }
}