using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Zone.Commands.Models;

public class UpdateZoneCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }
    public string Name { get; set; }
    
}