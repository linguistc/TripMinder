using AutoMapper;
using TripMinder.Core.Features.Zone.Commands.Models;
using TripMinder.Core.Features.Zone.Queries.Responses;

namespace TripMinder.Core.Mapping.Zone;

public partial class ZoneProfile : Profile
{
    public ZoneProfile()
    {
        this.GetZoneByIdMapping();
        this.GetZonesListMapping();
        this.CreateZoneMapping();
        this.UpdateZoneMapping();
    }

    private void UpdateZoneMapping()
    {
        CreateMap<UpdateZoneCommand, Data.Entities.Zone>()
            .ForMember(dest => dest.Id,
                options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name));

    }

    private void CreateZoneMapping()
    {
        CreateMap<CreateZoneCommand, Data.Entities.Zone>()
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name));

    }

    private void GetZonesListMapping()
    {
        CreateMap<Data.Entities.Zone, GetZonesListResponse>()
            .ForMember(dest => dest.Id,
                options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name));

    }

    private void GetZoneByIdMapping()
    {
        CreateMap<Data.Entities.Zone, GetZoneByIdResponse>()
            .ForMember(dest => dest.Id,
                options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                options => options.MapFrom(src => src.Name));
    }
}