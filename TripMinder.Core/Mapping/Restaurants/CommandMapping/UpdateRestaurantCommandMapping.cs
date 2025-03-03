using TripMinder.Core.Features.Restaurants.Commands.Models;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.Restaurants;

public partial class RestaurantProfile
{
    public void UpdateRestaurantCommandMapping() 
    {
        CreateMap<UpdateRestaurantCommand, Restaurant>()
            .ForMember(dest => dest.Description,
                options => options.MapFrom(src => src.DescriptionId))
            .ForMember(dest => dest.Class,
                options => options.MapFrom(src => src.ClassId))
            .ForMember(dest => dest.Zone,
                options => options.MapFrom(src => src.ZoneId ));
    }
}