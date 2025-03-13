using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.AccomodationClass.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.AccomodationClass.Commands.Handlers;

public class AccomodationClassCommandHandler : RespondHandler
, IRequestHandler<CreateAccomodationClassCommand, Respond<string>>
, IRequestHandler<UpdateAccomodationClassCommand, Respond<string>>
, IRequestHandler<DeleteAccomodationClassCommand, Respond<string>>
{
    #region Fields
    private readonly IAccomodationClassService accomodationClassService;
    private readonly IMapper mapper;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;
    #endregion

    #region Constructors
    public AccomodationClassCommandHandler(IAccomodationClassService accomodationClassService, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
    {
        this.accomodationClassService = accomodationClassService;
        this.mapper = mapper;
        this.stringLocalizer = stringLocalizer;
    }
    #endregion
    
    public async Task<Respond<string>> Handle(CreateAccomodationClassCommand request, CancellationToken cancellationToken)
    {
        var accomodationClassMapper = this.mapper.Map<Data.Entities.AccomodationClass>(request);
        var result = await this.accomodationClassService.CreateAsync(accomodationClassMapper);
        if (result == "Created") return Created("Created");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(UpdateAccomodationClassCommand request, CancellationToken cancellationToken)
    {
        var accomodationClass = await this.accomodationClassService.GetAccomodationClassByIdAsync(request.Id);
        if (accomodationClass == null) return NotFound<string>();
        
        var accomodationClassMapper = this.mapper.Map(request, accomodationClass);
        var result = await this.accomodationClassService.UpdateAsync(accomodationClassMapper);
        if (result == "Updated") return Success($"{this.stringLocalizer[SharedResourcesKeys.Updated]} {accomodationClass.Id}");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(DeleteAccomodationClassCommand request, CancellationToken cancellationToken)
    {
        var accomodationClass = await this.accomodationClassService.GetAccomodationClassByIdAsync(request.Id);
        if (accomodationClass == null) return NotFound<string>();
        
        var result = await this.accomodationClassService.DeleteAsync(accomodationClass);
        if (result == "Deleted") return Deleted<string>($"{this.stringLocalizer[SharedResourcesKeys.Deleted]} {accomodationClass.Id}");
        else return BadRequest<string>();
    }
}