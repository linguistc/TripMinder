
using TripMinder.Core.DTOHelpers;
namespace TripMinder.Core.Features.Entertainments.Queries.Responses
{
    public class GetEntertainmentsListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string ClassType { get; set; }
        public string Zone { get; set; }
        public int ZoneId { get; set; }
        public string Governorate { get; set; }
        public int GovernorateId { get; set; }
        public double Rating { get; set; }
        public double AveragePricePerAdult { get; set; }
        public string EntertainmentType { get; set; }
        public bool HasKidsArea { get; set; }
        public string Address { get; set; }
        public string? MapLink { get; set; }
        
        public string? ContactLink { get; set; } 
        
        public string? ImageUrl { get; set; }
        public string PlaceType { get; set; }

        public float Score { get; set; }

    }
}
