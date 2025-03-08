using TripMinder.Core.DTOHelpers;

namespace TripMinder.Core.Features.TourismAreas.Queries.Responses
{
    public class GetSingleTourismAreaResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public int NumOfBeds { get; set; }
        public int NumOfPersons { get; set; }
        public string Class { get; set; }
        public string Zone { get; set; }
        public double AveragePricePerAdult { get; set; }
        public string Category { get; set; }
        public bool HasKidsArea { get; set; }
        public LocationDto Location { get; set; }
        public List<string> Images { get; set; }
        public List<SocialProfileDto> BusinessSocialProfiles { get; set; }


    }
}
