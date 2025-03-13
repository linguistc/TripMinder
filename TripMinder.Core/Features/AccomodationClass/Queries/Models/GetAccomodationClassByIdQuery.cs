using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.AccomodationClass.Queries.Responses;

namespace TripMinder.Core.Features.AccomodationClass.Queries.Models;

public class GetAccomodationClassByIdQuery : IRequest<Respond<GetAccomodationClassByIdResponse>>
{
    public int Id { get; set; }

    public GetAccomodationClassByIdQuery(int id)
    {
        this.Id = id;
    }
}