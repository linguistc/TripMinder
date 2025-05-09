using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;
using TripMinder.Core.Behaviors;

namespace TripMinder.Core.Features.TourismAreas.Queries.Handlers
{


    public class TourismAreasQueryHandler : RespondHandler
                                        , IRequestHandler<GetTourismAreasListQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreasListByZoneIdQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreasListByGovernorateIdQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreaByIdQuery, Respond<GetTourismAreaByIdResponse>>
                                        , IRequestHandler<GetTourismAreasListByClassIdQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreasListByTypeIdQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreasListByRatingQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreasListLessThanPriceQuery, Respond<List<GetTourismAreasListResponse>>>
                                        , IRequestHandler<GetTourismAreasListMoreThanPriceQuery, Respond<List<GetTourismAreasListResponse>>>
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

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListQuery request, CancellationToken cancellationToken)
        {
            var tourismAreas = await service.GetTourismAreasListAsync();

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

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListByZoneIdQuery request, CancellationToken cancellationToken)
        {
            var tourismAreasList = await this.service.GetTourismAreasListByZoneIdAsync(request.ZoneId, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListByGovernorateIdQuery request, CancellationToken cancellationToken)
        {
            var tourismAreasList = await this.service.GetTourismAreasListByGovernorateIdAsync(request.GovernorateId, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListByClassIdQuery request, CancellationToken cancellationToken)
        {
            var tourismAreasList = await this.service.GetTourismAreasListByClassIdAsync(request.ClassId, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var tourismAreasList = await this.service.GetTourismAreasByTypeIdAsync(request.TypeId, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);            

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListByRatingQuery request, CancellationToken cancellationToken)
        {
            
            var tourismAreasList = await this.service.GetTourismAreasByRatingAsync(request.Rating, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            
            return result;
            
        }

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListLessThanPriceQuery request, CancellationToken cancellationToken)
        {
            var tourismAreasList = await this.service.GetTourismAreasLessThanPriceAsync((double)request.Price, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetTourismAreasListMoreThanPriceQuery request, CancellationToken cancellationToken)
        {
            var tourismAreasList = await this.service.GetTourismAreasMoreThanPriceAsync((double)request.Price, cancellationToken);

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreasList);

            var result = Success(tourismAreaMapper);

            result.Meta = new { Count = tourismAreaMapper.Count };
            return result;
        }
        #endregion
    }
}
