using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.AccomodationClass.Commands.Models;

public class CreateAccomodationClassCommand : IRequest<Respond<string>>
{
    public string Type { get; set; }
}