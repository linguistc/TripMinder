using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Governorates.Queries.Responses;

namespace TripMinder.Core.Features.Governorates.Queries.Models;

public class GetGovernoratesListQuery : IRequest<Respond<List<GetGovernoratesListResponse>>>
{
}