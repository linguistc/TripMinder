using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripMinder.Core.Mapping.Restaurants
{
    public partial class RestaurantProfile : Profile
    {
        public RestaurantProfile() 
        {
            this.GetSingleRestaurantMapping();
            this.GetRestaurantsListMapping();
        }
    }
}
