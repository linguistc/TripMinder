
using TripMinder.Core.DTOHelpers;
namespace TripMinder.Core.Features.Entertainments.Queries.Responses
{
    public class GetAllEntertainmentsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
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
