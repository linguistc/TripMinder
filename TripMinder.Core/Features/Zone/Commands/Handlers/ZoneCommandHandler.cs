using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Zone.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;
using TripMinder.Data.Entities;

namespace TripMinder.Core.Features.Zone.Commands.Handlers;

public class ZoneCommandHandler : RespondHandler
                                , IRequestHandler<CreateZoneCommand, Respond<string>>
                                , IRequestHandler<UpdateZoneCommand, Respond<string>>
                                , IRequestHandler<DeleteZoneCommand, Respond<string>>
{
    #region Fields
    private readonly IMapper mapper;
    private readonly IZoneService zoneService;
    private readonly IStringLocalizer<SharedResources> stringlocalizer;
    #endregion


    #region Constructors
    public ZoneCommandHandler(IMapper mapper, IZoneService zoneService, IStringLocalizer<SharedResources> stringlocalizer) : base(stringlocalizer)
    {
        this.mapper = mapper;
        this.zoneService = zoneService;
        this.stringlocalizer = stringlocalizer;
    }
    #endregion


    #region Methods

    public async Task<Respond<string>> Handle(CreateZoneCommand request, CancellationToken cancellationToken)
    {
        var zoneMapper = this.mapper.Map<Data.Entities.Zone>(request);
        
        var result = await this.zoneService.CreateAsync(zoneMapper);

        if (result == "Created") return Created("");

        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(UpdateZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = await this.zoneService.GetZoneByIdAsync(request.Id);
        if(zone == null) return NotFound<string>();
        
        var zoneMapper = this.mapper.Map(request, zone);
        
        var result = await this.zoneService.UpdateAsync(zoneMapper);
        if (result == "Updated") return Success($"{this.stringlocalizer[SharedResourcesKeys.Updated]} {zone.Id}");
        
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(DeleteZoneCommand request, CancellationToken cancellationToken)
    {
        var zone = await this.zoneService.GetZoneByIdAsync(request.Id);
        if(zone == null) return NotFound<string>();
        
        var result = await this.zoneService.DeleteAsync(zone);
        if (result == "Deleted") return Deleted<string>($"{this.stringlocalizer[SharedResourcesKeys.Deleted]} {request.Id}");
        
        else return BadRequest<string>();
    }
    
    #endregion
}