using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Data.Entities;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.TourismAreas.Commands.Handlers;

public class TourismAreaCommandHandler : RespondHandler 
                                        , IRequestHandler<CreateTourismAreaCommand, Respond<string>>
                                        , IRequestHandler<UpdateTourismAreaCommand, Respond<string>>
                                        , IRequestHandler<DeleteTourismAreaCommand, Respond<string>>
{
    #region Fields
    private readonly ITourismAreaService tourismAreaService;
    private readonly IMapper mapper;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;
    #endregion

    #region Constructors

    public TourismAreaCommandHandler(IMapper mapper, ITourismAreaService tourismAreaService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
    {
        this.mapper = mapper;
        this.tourismAreaService = tourismAreaService;
        this.stringLocalizer = stringLocalizer;
    }
    #endregion

    #region Methods
    
    public async Task<Respond<string>> Handle(CreateTourismAreaCommand request, CancellationToken cancellationToken)
    {
        var tourismAreaMapper = this.mapper.Map<TourismArea>(request);

        var result = await this.tourismAreaService.CreateAsync(tourismAreaMapper);

        if (result == "Created") return Created("");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(UpdateTourismAreaCommand request, CancellationToken cancellationToken)
    {
        var tourismArea = await this.tourismAreaService.GetTourismAreaByIdAsync(request.Id);
        if (tourismArea == null) return NotFound<string>();

        var tourismAreaMapper = this.mapper.Map<TourismArea>(request);

        var result = await this.tourismAreaService.UpdateAsync(tourismAreaMapper);

        if (result == "Updated") return Success($"{this.stringLocalizer[SharedResourcesKeys.Updated]} {tourismAreaMapper.Id}");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(DeleteTourismAreaCommand request, CancellationToken cancellationToken)
    {
        var tourismArea = await this.tourismAreaService.GetTourismAreaByIdAsync(request.Id);
        if (tourismArea == null) return NotFound<string>();

        var result = await this.tourismAreaService.DeleteAsync(tourismArea);    

        if (result == "Deleted") return Success($"{this.stringLocalizer[SharedResourcesKeys.Deleted]} {tourismArea.Id}");
        else return BadRequest<string>();
    }
    #endregion

}