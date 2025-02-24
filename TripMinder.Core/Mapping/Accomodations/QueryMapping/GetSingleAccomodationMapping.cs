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
            CreateMap<Accomodation, GetSingleAccomodationResponse>()
                .ForMember(dest => dest.Description,
                            options => options.MapFrom(src => src.Description.Text))
                .ForMember(dest => dest.Class,
                            options => options.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.Zone,
                            options => options.MapFrom(src => src.Zone.Name));
        }
    }
}
