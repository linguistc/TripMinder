public class GetAllEntertainmentsResponse : RespondHandler
{
    public List<EntertainmentDto> Entertainments { get; set; }
}

public class EntertainmentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; }
}