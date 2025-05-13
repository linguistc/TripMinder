using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Governorates.Queries.Models;
using TripMinder.Core.Features.Governorates.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Governorates.Queries.Handlers;

public class GovernorateQueryHandler : RespondHandler   
, IRequestHandler<GetGovernoratesListQuery, Respond<List<GetGovernoratesListResponse>>>
, IRequestHandler<GetGovernorateByIdQuery, Respond<GetGovernorateByIdResponse>>
{
    #region Fields
    private readonly IMapper mapper;
    private readonly IGovernorateService governorateService;
    private readonly IStringLocalizer<SharedResources> stringLocalizer; 
    #endregion


    #region Constructor
    public GovernorateQueryHandler(
        IMapper mapper,
        IGovernorateService governorateService,
        IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)    
    {
        this.mapper = mapper;
        this.governorateService = governorateService;
        this.stringLocalizer = stringLocalizer;
    }
    #endregion


    #region Methods

    public async Task<Respond<List<GetGovernoratesListResponse>>> Handle(GetGovernoratesListQuery request, CancellationToken cancellationToken)
    {
        
        var governorates = await this.governorateService.GetGovernoratesListAsync();
        var response = this.mapper.Map<List<GetGovernoratesListResponse>>(governorates);
        var result = Success(response);
        
        result.Meta = new { Count = response.Count };

        return result; 
    }

    public async Task<Respond<GetGovernorateByIdResponse>> Handle(GetGovernorateByIdQuery request, CancellationToken cancellationToken)
    {
        
        var governorate = await this.governorateService.GetGovernorateByIdAsync(request.Id);
        if(governorate == null)
            return NotFound<GetGovernorateByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

        var result = this.mapper.Map<GetGovernorateByIdResponse>(governorate);
        return Success(result);
    }
    #endregion
}
