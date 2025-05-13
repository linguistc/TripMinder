using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Governorates.Queries.Responses;

namespace TripMinder.Core.Features.Governorates.Queries.Models;

public class GetGovernorateByIdQuery : IRequest<Respond<GetGovernorateByIdResponse>>
{
    public int Id { get; set; }

    public GetGovernorateByIdQuery(int id)
    {
        this.Id = id;
    }
}