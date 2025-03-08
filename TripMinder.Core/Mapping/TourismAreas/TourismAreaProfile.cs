using AutoMapper;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Mapping.TourismAreas
{


    public partial class TourismAreaProfile : Profile
    {

        public TourismAreaProfile()
        {
            this.GetSingleTourismAreaMapping();
            this.GetAllTourismAreaMapping();
            this.CreateTourismAreaCommandMapping();
            this.UpdateTourismAreaCommandMapping();
        }
    }
}
