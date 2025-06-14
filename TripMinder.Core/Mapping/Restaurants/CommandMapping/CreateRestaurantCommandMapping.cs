using Microsoft.Extensions.Options;
using TripMinder.Core.Features.Restaurants.Commands.Models;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.Restaurants;

public partial class RestaurantProfile
{
    public void CreateRestaurantMapping() 
    {
        CreateMap<CreateRestaurantCommand, Restaurant>()
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(dest => dest.ClassId,
                options => options.MapFrom(src => src.ClassId))
            .ForMember(dest => dest.ZoneId,
                options => options.MapFrom(src => src.ZoneId ))
            .ForMember(dest => dest.PlaceTypeId, 
                options => options.MapFrom(src => src.PlaceTypeId))
            .ForMember(dest => dest.Address,
                options => options.MapFrom(src => src.Address))
            .ForMember(dest => dest.HasKidsArea,
                options => options.MapFrom(src => src.HasKidsArea))
            .ForMember(dest => dest.AveragePricePerAdult,
                options => options.MapFrom(src => src.AveragePricePerAdult))
            .ForMember(dest => dest.ContactLink,
                options => options.MapFrom(src => src.ContactLink))
            .ForMember(dest => dest.MapLink,
                options => options.MapFrom(src => src.MapLink))
            .ForMember(dest => dest.Description,
                opions => opions.MapFrom(src => src.Description))
            .ForMember(dest => dest.FoodCategoryId,
                options => options.MapFrom(src => src.FoodCategoryId));
    }
}