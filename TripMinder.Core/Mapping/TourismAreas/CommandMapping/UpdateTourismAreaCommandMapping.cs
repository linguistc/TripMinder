using TripMinder.Core.Features.TourismAreas.Commands.Models;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.TourismAreas
{
    public partial class TourismAreaProfile
    {
        public void UpdateTourismAreaCommandMapping()
        {

            CreateMap<UpdateTourismAreaCommand, TourismArea>()
                .ForMember(dest => dest.Description,
                    options => options.MapFrom(src => src.DescriptionId))
                .ForMember(dest => dest.Class,
                    options => options.MapFrom(src => src.ClassId))
                .ForMember(dest => dest.Zone,
                    options => options.MapFrom(src => src.ZoneId))
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
}