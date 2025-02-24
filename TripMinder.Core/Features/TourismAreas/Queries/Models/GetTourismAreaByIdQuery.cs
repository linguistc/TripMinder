using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;

public class GetTourismAreaByIdQuery : IRequest<Respond<GetSingleTourismAreaResponse>>
{
    public int Id { get; set; }

    public GetTourismAreaByIdQuery(int id)
    {
        this.Id = id;
    }
}