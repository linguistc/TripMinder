using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.TourismAreas.Commands.Models;

public class DeleteTourismAreaCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }

    public DeleteTourismAreaCommand(int id)
    {
        this.Id = id;
    }
    
}