
namespace TripMinder.Core.Features.TourismAreas.Queries.Responses
{
    public class GetTourismAreasListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string ClassType { get; set; }
        public string Zone { get; set; }
        public string Governorate { get; set; }
        public double Rating { get; set; }
        public double AveragePricePerAdult { get; set; }
        public string TourismType { get; set; }
        public bool HasKidsArea { get; set; }
        public string Address { get; set; }
        public string? MapLink { get; set; }
        
        public string? ContactLink { get; set; } 
        
        public string? ImageSource { get; set; }
        public string PlaceType { get; set; }
        
        public float Score { get; set; }

    }
}
