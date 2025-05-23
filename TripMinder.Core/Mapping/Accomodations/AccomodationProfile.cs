﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripMinder.Core.Mapping.Accomodations
{
    public partial class AccomodationProfile : Profile
    {
        public AccomodationProfile() 
        {
            this.GetAccomodationByIdMapping();
            this.GetAccomdationsListMapping();
            this.CreateAccomodationCommandMapping();
            this.UpdateAccomodationCommandMapping();
        }
    }
}
