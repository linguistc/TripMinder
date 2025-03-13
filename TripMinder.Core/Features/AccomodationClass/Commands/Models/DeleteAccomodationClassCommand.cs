using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.AccomodationClass.Commands.Models;

public class DeleteAccomodationClassCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }

    public DeleteAccomodationClassCommand(int id)
    {
        this.Id = id;
    }
}