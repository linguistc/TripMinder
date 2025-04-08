
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.TourismAreas
{
    public partial class TourismAreaProfile
    {

        void GetTourismAreasListMapping()
        {
            CreateMap<TourismArea, GetTourismAreasListResponse>()
                .ForMember(dest => dest.Id,
                    options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, 
                    options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.TourismType, 
                    options => options.MapFrom(src => src.TourismType.Type))
                .ForMember(dest => dest.ClassType, 
                    options => options.MapFrom(src => src.Class.Type))
                .ForMember(dest => dest.Zone, 
                    options => options.MapFrom(src => src.Zone.Name ))
                .ForMember(dest => dest.Governorate, 
                    options => options.MapFrom(src => src.Zone.Governorate.Name))
                .ForMember(dest => dest.Rating,
                    options => options.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Score,
                    options => options.MapFrom(src => src.Score))
                .ForMember(dest => dest.PlaceType, 
                    options => options.MapFrom(src => src.PlaceType.Type))
                .ForMember(dest => dest.AveragePricePerAdult, 
                    options => options.MapFrom(src => src.AveragePricePerAdult))
                .ForMember(dest => dest.HasKidsArea, 
                    options => options.MapFrom(src => src.HasKidsArea))
                .ForMember(dest => dest.Description, 
                    options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.Address, 
                    options => options.MapFrom(src => src.Address))
                .ForMember(dest => dest.MapLink, 
                    options => options.MapFrom(src => src.MapLink))
                .ForMember(dest => dest.ContactLink, 
                    options => options.MapFrom(src => src.ContactLink))
                .ForMember(dest => dest.ImageSource, 
                    options => options.MapFrom(src => src.ImageSource));
            
        }
    }
}