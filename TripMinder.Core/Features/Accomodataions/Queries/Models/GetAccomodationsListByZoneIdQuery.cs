using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;

namespace TripMinder.Core.Features.Accomodataions.Queries.Models;

public class GetAccomodationsListByZoneIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int ZoneId { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByZoneIdQuery(int zoneId, int priority) => (this.ZoneId, this.Priority) = (zoneId, priority);
}

public class GetAccomodationsListByTypeIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int TypeId { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByTypeIdQuery(int typeId, int priority) => (this.TypeId, this.Priority) = (typeId, priority);
}

public class GetAccomodationsListByClassIdQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int ClassId { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByClassIdQuery(int classId, int priority) => (this.ClassId, this.Priority) = (classId, priority);
}

public class GetAccomodationsListByRatingQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public int Rating { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByRatingQuery(int rating, int priority) => (this.Rating, this.Priority) = (rating, priority);
}

public class GetAccomodationsListByNumOfBedsQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public short NumOfBeds { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByNumOfBedsQuery(short numOfBeds, int priority) => (this.NumOfBeds, this.Priority) = (numOfBeds, priority);
}

public class GetAccomodationsListByNumOfAdultsQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public short NumOfAdults { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListByNumOfAdultsQuery(short numOfAdults, int priority) => (this.NumOfAdults, this.Priority) = (numOfAdults, priority);
}

public class GetAccomodationsListLessThanPriceQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public decimal Price { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListLessThanPriceQuery(decimal price, int priority) => (this.Price, this.Priority) = (price, priority);
}

public class GetAccomodationsListMoreThanPriceQuery : IRequest<Respond<List<GetAccomodationsListResponse>>>
{
    public decimal Price { get; set; }
    public int Priority { get; set; }
        
    public GetAccomodationsListMoreThanPriceQuery(decimal price, int priority) => (this.Price, this.Priority) = (price, priority);
}

