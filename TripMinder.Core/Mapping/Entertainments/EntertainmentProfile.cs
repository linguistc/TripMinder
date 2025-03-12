using AutoMapper;

namespace TripMinder.Core.Mapping.Entertainments
{

    public partial class EntertainmentProfile : Profile
    {
        public EntertainmentProfile()
        {
            this.GetEntertainmentByIdMapping();
            this.GetEntertainmentsListMapping();
            this.CreateEntertainmentCommandMapping();
            this.UpdateEntertainmentCommandMapping();
        }
    }

}