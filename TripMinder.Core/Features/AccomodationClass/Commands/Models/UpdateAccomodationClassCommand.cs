using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.AccomodationClass.Commands.Models;

public class UpdateAccomodationClassCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }
    public string Type { get; set; }
}