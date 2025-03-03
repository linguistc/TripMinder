namespace TripMinder.Core.Features.Restaurants.Queries.Responses
{
    public class GetRestaurantByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Class { get; set; }

        public string Zone { get; set; }

        public double AveragePricePerAdult { get; set; }

        public bool HasKidsArea { get; set; }
    }
}
