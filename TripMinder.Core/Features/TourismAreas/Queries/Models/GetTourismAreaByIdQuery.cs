public class GetTourismAreaByIdQuery : IRequest<GetSingleTourismAreaResponse>
{
    public int Id { get; set; }

    public GetTourismAreaByIdQuery(int id)
    {
        this.Id = id;
    }
}