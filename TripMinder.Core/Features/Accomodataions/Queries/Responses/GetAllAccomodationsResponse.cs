namespace TripMinder.Core.Features.Accomodataions.Queries.Responses
{
    public class GetAllAccomodationsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int NumOfBeds { get; set; }
        public int NumOfPersons { get; set; }

        public string Location { get; set; }

        public string Class { get; set; }

        public string Zone { get; set; }

        public double AveragePricePerAdult { get; set; }

        public bool HasKidsArea { get; set; }

    }
}
