using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Commands.Models;
using TripMinder.Core.Resources;
using TripMinder.Data.Entities;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Accomodataions.Commands.Handlers;

public class AccomodationCommandHandler : RespondHandler
                                        , IRequestHandler<CreateAccomodationCommand, Respond<string>>
                                        , IRequestHandler<UpdateAccomodationCommand, Respond<string>>
                                        , IRequestHandler<DeleteAccomodationCommand, Respond<string>>
{
    #region Fields

    private readonly IMapper mapper;
    private readonly IAccomodationService accomodationservice;
    private readonly IStringLocalizer<SharedResources> StringLocalizer;
    #endregion

    #region Constructors

    public AccomodationCommandHandler(IMapper mapper, IAccomodationService accomodationservice, IStringLocalizer<SharedResources> stringLocalizer)
    {
        this.mapper = mapper;
        this.accomodationservice = accomodationservice;
        this.StringLocalizer = stringLocalizer;
    }
    #endregion

    #region Methods

    

    public async Task<Respond<string>> Handle(CreateAccomodationCommand request, CancellationToken cancellationToken)
    {
        var accomodationMapper = this.mapper.Map<Accomodation>(request);
        
        var result = await this.accomodationservice.CreateAsync(accomodationMapper);

        if (result == "Created") return Created("");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(UpdateAccomodationCommand request, CancellationToken cancellationToken)
    {
        var accomodationMapper = this.mapper.Map<Accomodation>(request);

        var result = await this.accomodationservice.UpdateAsync(accomodationMapper);

        if (result == "Updated") return Success($"{this.StringLocalizer[SharedResourcesKeys.Updated]} {accomodationMapper.Id}");
        else return BadRequest<string>();
    }

    public async Task<Respond<string>> Handle(DeleteAccomodationCommand request, CancellationToken cancellationToken)
    {
        var accomodation = await this.accomodationservice.GetAccomodationByIdAsync(request.Id); // withInclude
        if (accomodation == null) return NotFound<string>();
        var result = await this.accomodationservice.DeleteAsync(accomodation);
        if (result == "Deleted") return Deleted<string>($"{this.StringLocalizer[SharedResourcesKeys.Deleted]} {accomodation.Id}");
        else return BadRequest<string>();
    }
    
    #endregion
}