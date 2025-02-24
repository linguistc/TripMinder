public class GetAllTourismAreasResponse : RespondHandler
{
    public List<TourismAreaDto> TourismAreas { get; set; }
}

public class TourismAreaDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string Type { get; set; }
    public decimal EntryFee { get; set; }
    public string VisitingHours { get; set; }
}