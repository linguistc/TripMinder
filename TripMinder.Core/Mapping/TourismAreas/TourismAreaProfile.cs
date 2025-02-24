using AutoMapper;
using TripMinder.Data.Entities;

public partial class TourismAreaProfile : Profile
{

    public TourismAreaProfile() 
    {
        this.GetSingleTourismAreaMapping();
        this.GetAllTourismAreaMapping();

    }
}
