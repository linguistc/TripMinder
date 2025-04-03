using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.DTOHelpers;

namespace TripMinder.Core.Features.Accomodataions.Commands.Models;

public class UpdateAccomodationCommand : IRequest<Respond<string>>
{
    public int Id { get; set; }
    public string Name { get; set; }
        
    public int ClassTypeId { get; set; }

    public string Description { get; set; }
    
    public int AccomodationTypeId { get; set; }

    public int ZoneId { get; set; }
    public double AveragePricePerAdult { get; set; }

    public string Address { get; set; } 
    public string? MapLink { get; set; }
        
    public string? ContactLink { get; set; } 
        
    public string? ImageSource { get; set; }
    public int PlaceTypeId { get; set; }
    public bool HasKidsArea { get; set; }
    public int NumOfBeds { get; set; }
    public int NumOfPersons { get; set; }
    public string BedsStatus { get; set; }

}