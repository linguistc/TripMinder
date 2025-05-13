namespace TripMinder.Core.Features.Governorates.Queries.Responses;

public class GetGovernorateByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string? ImageUrl { get; set; }

}