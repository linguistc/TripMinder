using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.DTOHelpers;

namespace TripMinder.Core.Features.Restaurants.Commands.Models
{
    public class CreateRestaurantCommand : IRequest<Respond<string>>
    {
        // public string NameAr { get; set; }
        public string Name { get; set; }
        
        public int FoodCategoryId { get; set; }
        public int ClassId { get; set; }

        public string Description { get; set; }
        
        // public string DescriptionAr { get; set; }
        public int ZoneId { get; set; }
        public double AveragePricePerAdult { get; set; }

        public string Address { get; set; } 
        public string? MapLink { get; set; }
        
        public string? ContactLink { get; set; } 
        
        public string? ImageSource { get; set; }
        public int PlaceTypeId { get; set; }
        public bool HasKidsArea { get; set; }

    }
}
