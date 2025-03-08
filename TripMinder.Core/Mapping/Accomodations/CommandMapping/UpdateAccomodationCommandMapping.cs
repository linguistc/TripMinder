using TripMinder.Core.Features.Accomodataions.Commands.Models;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.Accomodations;

public partial class AccomodationProfile
{
    public void UpdateAccomodationCommandMapping()
    {
        CreateMap<UpdateAccomodationCommand, Accomodation>()
            .ForMember(dest => dest.Description,
                options => options.MapFrom(src => src.DescriptionId))
            .ForMember(dest => dest.Class,
                options => options.MapFrom(src => src.ClassId))
            .ForMember(dest => dest.Zone,
                options => options.MapFrom(src => src.ZoneId ))
            .ForMember(dest => dest.NumOfBeds,
                options => options.MapFrom(src => src.NumOfBeds))
            .ForMember(dest => dest.NumOfMembers,
                options => options.MapFrom(src => src.NumOfPersons))
            .ForMember(dest => dest.PlaceCategory, 
                options => options.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.Location,
                options => options.MapFrom(src => src.Location))
            .ForMember(dest => dest.Images,
                options => options.MapFrom(src => src.Images))
            .ForMember(dest => dest.BusinessSocialProfiles,
                options => options.MapFrom(src => src.BusinessSocialProfiles))
            .ForMember(dest => dest.HasKidsArea,
                options => options.MapFrom(src => src.HasKidsArea));


    }
}