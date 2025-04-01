using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.TourismAreas.Queries.Handlers
{


    public class TourismAreasQueryHandler : RespondHandler
                                        , IRequestHandler<GetAllTourismAreasQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreaByIdQuery, Respond<GetTourismAreaByIdResponse>>
    {
        #region Fields
        private readonly IMapper mapper;
        private readonly ITourismAreaService service;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        #endregion

        #region Constructors

        public TourismAreasQueryHandler(IMapper mapper, ITourismAreaService service, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.service = service;
            this.stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Methods

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetAllTourismAreasQuery request, CancellationToken cancellationToken)
        {
            var tourismAreas = await service.GetAllTourismAreasAsync();

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreas);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }

        public async Task<Respond<GetTourismAreaByIdResponse>> Handle(GetTourismAreaByIdQuery request, CancellationToken cancellationToken)
        {
            var tourismArea = await service.GetTourismAreaByIdWithIncludeAsync(request.Id);

            if (tourismArea == null)
                return NotFound<GetTourismAreaByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = this.mapper.Map<GetTourismAreaByIdResponse>(tourismArea);

            return Success(result);
        }

        #endregion
    }
}