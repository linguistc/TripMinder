using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Zone.Queries.Models;
using TripMinder.Core.Features.Zone.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Zone.Queries.Handlers;

public class ZoneQueryHandler : RespondHandler
    , IRequestHandler<GetZoneByIdQuery, Respond<GetZoneByIdResponse>>
    , IRequestHandler<GetZonesListQuery, Respond<List<GetZonesListResponse>>>
{
    #region Fields
    private readonly IZoneService zoneService;
    private readonly IMapper mapper;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;
    #endregion

    #region Constructors
    public ZoneQueryHandler(
        IZoneService zoneService,
        IMapper mapper,
        IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
    {
        this.zoneService = zoneService;
        this.mapper = mapper;
        this.stringLocalizer = stringLocalizer;
    }
    #endregion

    #region Methods
    public async Task<Respond<GetZoneByIdResponse>> Handle(GetZoneByIdQuery request, CancellationToken cancellationToken)
    {
        var zone = await this.zoneService.GetZoneByIdAsync(request.Id);

        if (zone == null)
            return NotFound<GetZoneByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

        var result = this.mapper.Map<GetZoneByIdResponse>(zone);
        return Success(result);
    }

    public async Task<Respond<List<GetZonesListResponse>>> Handle(GetZonesListQuery request, CancellationToken cancellationToken)
    {
        var zonesList = await this.zoneService.GetZonesListAsync();

        var zoneMapper = this.mapper.Map<List<GetZonesListResponse>>(zonesList);

        var result = Success(zoneMapper);
        result.Meta = new { Count = zoneMapper.Count };

        return result;
    }
    #endregion
}