public class GetEntertainmentByIdQuery : IRequest<GetSingleEntertainmentResponse>
{
    public int Id { get; set; }

    public GetEntertainmentByIdQuery(int id)
    {
        this.Id = id;
    }
}