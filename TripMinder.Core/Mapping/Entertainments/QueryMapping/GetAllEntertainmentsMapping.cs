using TripMinder.Core.Features.Entertainments.Queries.Responses;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.Entertainments
{

    public partial class EntertainmentProfile
    {
        void GetAllEntertainmentsMapping()
        {
            CreateMap<Entertainment, GetAllEntertainmentsResponse>()
                .ForMember(dest => dest.Description,
                            options => options.MapFrom(src => src.Description.Text))
                .ForMember(dest => dest.Class,
                            options => options.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.Zone,
                            options => options.MapFrom(src => src.Zone.Name));
        }
    }

}