
namespace TripMinder.Core.Features.Accomodataions.Queries.Responses
{
    public class GetAccomodationsListResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string AccomodationType { get; set; }
        public string ClassType { get; set; }
        public string Zone { get; set; }
        public string PlaceType { get; set; }
        public double AveragePricePerAdult { get; set; }
        public bool HasKidsArea { get; set; }
      
        
        public string? Description { get; set; }
        
        public string Address { get; set; }
        
        public string? MapLink { get; set; }
        
        public string? ContactLink { get; set; } 
        
        public string? ImageSource { get; set; }
        public int NumOfBeds { get; set; }
        public string? BedStatus { get; set; }
        public int NumOfPersons { get; set; }


    }
}
