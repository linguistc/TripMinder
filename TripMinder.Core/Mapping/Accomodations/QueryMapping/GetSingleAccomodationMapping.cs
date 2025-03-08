using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;
using TripMinder.Data.Entities;


namespace TripMinder.Core.Mapping.Accomodations
{
    public partial class AccomodationProfile
    {
        void GetSingleAccomodationMapping()
        {
            CreateMap<Accomodation, GetAccomodationByIdResponse>()
                .ForMember(dest => dest.Description,
                    options => options.MapFrom(src => src.Description.Text))
                .ForMember(dest => dest.Class,
                    options => options.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.Zone,
                    options => options.MapFrom(src => src.Zone.Name))
                .ForMember(dest => dest.Location,
                    options => options.MapFrom(src => new
                    {
                        src.Location.Latitude,
                        src.Location.Longitude,
                        src.Location.Address
                    }))
                .ForMember(dest => dest.Category,
                    options => options.MapFrom(src => src.PlaceCategory))
                .ForMember(dest => dest.AveragePricePerAdult,
                    options => options.MapFrom(src => src.AveragePricePerAdult))
                .ForMember(dest => dest.HasKidsArea, options => options.MapFrom(src => src.HasKidsArea))
                .ForMember(dest => dest.Images,
                    options => options.MapFrom(src => src.Images.Select(img => img.Source).ToList()))
                .ForMember(dest => dest.BusinessSocialProfiles,
                    options => options.MapFrom(src => src.BusinessSocialProfiles.Select(sp => new
                    {
                        sp.PlatformName,
                        sp.ProfileLink
                    }).ToList()));
        }
    }
}
