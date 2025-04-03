using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Zone.Commands.Models;

public class DeleteZoneCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }

    public DeleteZoneCommand(int id)
    {
        this.Id = id;
    }
}