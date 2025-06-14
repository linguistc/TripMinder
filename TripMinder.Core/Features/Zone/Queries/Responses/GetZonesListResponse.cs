namespace TripMinder.Core.Features.Zone.Queries.Responses;

public class GetZonesListResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string GovernorateName { get; set; }
    public int GovernorateId { get; set; }
}