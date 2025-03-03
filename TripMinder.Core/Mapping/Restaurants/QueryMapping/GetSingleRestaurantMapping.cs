using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Core.Features.Restaurants.Queries.Responses;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.Restaurants
{
    public partial class RestaurantProfile
    {
        void GetSingleRestaurantMapping()
        {
            CreateMap<Restaurant, GetRestaurantByIdResponse>()
                .ForMember(dest => dest.Description,
                            options => options.MapFrom(src => src.Description.Text))
                .ForMember(dest => dest.Class,
                            options => options.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.Zone,
                            options => options.MapFrom(src => src.Zone.Name));
                
        }

    }
}
