using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Data.Entities;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Entertainments.Commands.Handlers;

public class EntertainmentCommandHandler : RespondHandler
                                         , IRequestHandler<CreateEntertainmentCommand, Respond<string>>
                                         , IRequestHandler<UpdateEntertainmentCommand, Respond<string>>
                                         , IRequestHandler<DeleteEntertainmentCommand, Respond<string>>
{
    #region Fields

    private readonly IMapper mapper;
    private readonly IEntertainmentService entertainmentService;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;
    #endregion
    
    #region Constructors

    public EntertainmentCommandHandler(IMapper mapper, IEntertainmentService entertainmentService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
    {
        this.mapper = mapper;
        this.entertainmentService = entertainmentService;
        this.stringLocalizer = stringLocalizer;
    }
    #endregion

    #region Methods

    public async Task<Respond<string>> Handle(CreateEntertainmentCommand request, CancellationToken cancellationToken)
    {
        var entertainmentMapper = this.mapper.Map<Entertainment>(request);
        var result = await this.entertainmentService.CreateAsync(entertainmentMapper);
        
        if (result == "Created") return Created("");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(UpdateEntertainmentCommand request, CancellationToken cancellationToken)
    {
        var entertainment = await this.entertainmentService.GetEntertainmentByIdWithIncludeAsync(request.Id);
        if (entertainment == null) return NotFound<string>();
        
        var entertainmentMapper = this.mapper.Map(request, entertainment);
        var result = await this.entertainmentService.UpdateAsync(entertainmentMapper);
        
        
        if (result == "Updated") return Success($"{this.stringLocalizer[SharedResourcesKeys.Updated]} {entertainment.Id}");
        
        else return BadRequest<string>();
    }
    
    public async Task<Respond<string>> Handle(DeleteEntertainmentCommand request, CancellationToken cancellationToken)
    {
        var entertainment = await this.entertainmentService.GetEntertainmentByIdAsync(request.Id);
        if (entertainment == null) return NotFound<string>();
        
        var result = await this.entertainmentService.DeleteAsync(entertainment);
        if (result == "Deleted") return Deleted<string>($"{this.stringLocalizer[SharedResourcesKeys.Deleted]} {entertainment.Id}");
        else return BadRequest<string>();
    }
    #endregion

}