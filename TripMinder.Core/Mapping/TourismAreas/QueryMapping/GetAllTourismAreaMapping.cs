
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
using TripMinder.Data.Entities;

public partial class TourismAreaProfile
{

    void GetAllTourismAreaMapping()
    {
        CreateMap<TourismArea, GetAllTourismAreasResponse>()
                .ForMember(dest => dest.Description,
                            options => options.MapFrom(src => src.Description.Text))
                .ForMember(dest => dest.Class,
                            options => options.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.Zone,
                            options => options.MapFrom(src => src.Zone.Name));
       
    }
}