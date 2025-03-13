using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.AccomodationClass.Queries.Responses;

namespace TripMinder.Core.Features.AccomodationClass.Queries.Models;

public class GetAccomodationClassesListQuery : IRequest<Respond<List<GetAccomodationClassesListResponse>>>
{
    
}