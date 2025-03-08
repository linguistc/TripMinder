using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.DTOHelpers;

namespace TripMinder.Core.Features.Accomodataions.Commands.Models;

public class UpdateAccomodationCommand : IRequest<Respond<string>>
{
    public string Name { get; set; }

    public int DescriptionId { get; set; }
    public int NumOfBeds { get; set; }
    public int NumOfPersons { get; set; }
    public int ClassId { get; set; }
    public int ZoneId { get; set; }
    public double AveragePricePerAdult { get; set; }
    public int CategoryId { get; set; }
    public bool HasKidsArea { get; set; }
    public LocationDto Location { get; set; }
    public List<string> Images { get; set; }
    public List<SocialProfileDto> BusinessSocialProfiles { get; set; }

}