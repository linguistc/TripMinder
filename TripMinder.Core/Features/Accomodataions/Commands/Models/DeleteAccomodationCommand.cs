using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Accomodataions.Commands.Models;

public class DeleteAccomodationCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }

    public DeleteAccomodationCommand(int id)
    {
        this.Id = id;
    }
}