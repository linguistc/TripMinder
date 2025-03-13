using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.AccomodationClass.Queries.Models;
using TripMinder.Core.Features.AccomodationClass.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.AccomodationClass.Queries.Handlers;

public class AccomodationClassQueryHandler : RespondHandler
                                            , IRequestHandler<GetAccomodationClassByIdQuery, Respond<GetAccomodationClassByIdResponse>>
                                            , IRequestHandler<GetAccomodationClassesListQuery, Respond<List<GetAccomodationClassesListResponse>>>
{

    #region Fields
    private readonly IAccomodationClassService accomodationClassService;
    private readonly IMapper mapper;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;
    #endregion

    #region Constructors
    public AccomodationClassQueryHandler(IAccomodationClassService accomodationClassService, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
    {
        this.accomodationClassService = accomodationClassService;
        this.mapper = mapper;
        this.stringLocalizer = stringLocalizer;
        
    }
    #endregion

    #region Methods
    public async Task<Respond<GetAccomodationClassByIdResponse>> Handle(GetAccomodationClassByIdQuery request, CancellationToken cancellationToken)
    {
        var accomodationClass = await this.accomodationClassService.GetAccomodationClassByIdAsync(request.Id);

        if (accomodationClass == null)
            return NotFound<GetAccomodationClassByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

        var result = this.mapper.Map<GetAccomodationClassByIdResponse>(accomodationClass);
        return Success(result);
    }

    public async Task<Respond<List<GetAccomodationClassesListResponse>>> Handle(GetAccomodationClassesListQuery request, CancellationToken cancellationToken)
    {
        var accomodationClassesList = await this.accomodationClassService.GetAccomodationClassesListAsync();
        var accomodationClassMapper = this.mapper.Map<List<GetAccomodationClassesListResponse>>(accomodationClassesList);

        var result = Success(accomodationClassMapper);

        result.Meta = new { Count = accomodationClassMapper.Count };

        return result;
    }
    #endregion
}
