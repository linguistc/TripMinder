using MediatR;
using TripMinder.Core.Bases;

namespace TripMinder.Core.Features.Restaurants.Commands.Models
{
    public class CreateRestaurantCommand : IRequest<Respond<string>>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int DescriptionId { get; set; }
        public int ClassId { get; set; }

        public int ZoneId { get; set; }
        public double PricePerPerson { get; set; }

        public int CategoryId { get; set; }
        public bool HasKidsArea { get; set; }


    }
}
