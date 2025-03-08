using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.DTOHelpers;

namespace TripMinder.Core.Features.TourismAreas.Commands.Models;

public class UpdateTourismAreaCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int DescriptionId { get; set; }
    public int ClassId { get; set; }

    public int ZoneId { get; set; }
    public double PricePerPerson { get; set; }
    public int CategoryId { get; set; }
    public bool HasKidsArea { get; set; }
        
    public List<SocialProfileDto> BusinessSocialProfiles { get; set; }

    public LocationDto Location { get; set; }
    public List<string> Images { get; set; } // => How to create or edit the source?

    //public List<string> FoodCategories { get; set; } => handled in another Entity

    
}