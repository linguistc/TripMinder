using AutoMapper;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.TourismAreas
{


    public partial class TourismAreaProfile : Profile
    {

        public TourismAreaProfile()
        {
            this.GetTourismAreaByIdMapping();
            this.GetTourismAreasListMapping();
            this.CreateTourismAreaCommandMapping();
            this.UpdateTourismAreaCommandMapping();
        }
    }
}
