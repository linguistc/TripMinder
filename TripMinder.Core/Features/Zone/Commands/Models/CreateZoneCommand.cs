using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Zone.Commands.Models;

public class CreateZoneCommand : IRequest<Respond<string>>
{
    public string Name { get; set; }
    
}