using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Entertainments.Commands.Models;

public class DeleteEntertainmentCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }

    public DeleteEntertainmentCommand(int id)
    {
        this.Id = id;
    }
}