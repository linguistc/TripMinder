using AutoMapper;
using TripMinder.Core.Features.Governorates.Queries.Responses;

namespace TripMinder.Core.Mapping.Governorates;

public class GovernoratePofile : Profile
{
    public GovernoratePofile()
    {
        this.GetGovernorateByIdMapping();
        this.GetGovernoratesListMapping();
    }

    private void GetGovernoratesListMapping()
    {
        CreateMap<Data.Entities.Governorate, GetGovernoratesListResponse>()
            .ForMember(dest => dest.Id,
                options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name))
            .ForMember(dest => dest.ImageUrl,
                options => options.MapFrom(src => src.ImageUrl));
    }

    private void GetGovernorateByIdMapping()
    {
        CreateMap<Data.Entities.Governorate, GetGovernorateByIdResponse>()
            .ForMember(dest => dest.Id,
                options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name)).ForMember(dest => dest.ImageUrl,
                options => options.MapFrom(src => src.ImageUrl));
    }
}