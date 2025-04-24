using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Entertainments.Queries.Handlers
{

    

    public class EntertainmentQueryHandler : RespondHandler
                                        , IRequestHandler<GetEntertainmentsListQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentByIdQuery, Respond<GetEntertainmentByIdResponse>>
                                        , IRequestHandler<GetEntertainmentsListByZoneIdQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentsListByGovernorateIdQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentsListByClassIdQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentsListByTypeIdQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentsListByRatingQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentsListLessThanPriceQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentsListMoreThanPriceQuery, Respond<List<GetEntertainmentsListResponse>>>
    {
        #region Fields
        private readonly IMapper mapper;
        private readonly IEntertainmentService service;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        #endregion

        #region Constructors

        public EntertainmentQueryHandler(IMapper mapper, IEntertainmentService service, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.service = service;
            this.stringLocalizer = stringLocalizer;
        }

        #endregion

        #region Methods

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListQuery request, CancellationToken cancellationToken)
        {
            var entertainments = await service.GetEntertainmentsListAsync();

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainments);

            var result = Success(entertainmentMapper);
            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
        }

        public async Task<Respond<GetEntertainmentByIdResponse>> Handle(GetEntertainmentByIdQuery request, CancellationToken cancellationToken)
        {
            var entertainment = await service.GetEntertainmentByIdWithIncludeAsync(request.Id);

            if (entertainment == null)
                return NotFound<GetEntertainmentByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = this.mapper.Map<GetEntertainmentByIdResponse>(entertainment);

            return Success(result);
        }

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListByZoneIdQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListByZoneIdAsync(request.ZoneId, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
            
        }

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListByGovernorateIdQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListByGovernorateIdAsync(request.GovernorateId, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
        }


        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListByClassIdQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListByClassIdAsync(request.ClassId, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;        
        }

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListByTypeIdAsync(request.TypeId, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListByRatingQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListByRatingAsync(request.Rating, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListLessThanPriceQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListLessThanPriceAsync((double)request.Price, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetEntertainmentsListMoreThanPriceQuery request, CancellationToken cancellationToken)
        {
            var entertainmentsList = await this.service.GetEntertainmentsListMoreThanPriceAsync((double)request.Price, cancellationToken);

            entertainmentsList.ForEach(a => a.Score = CalculateScoreBehavior.CalculateScore(a.Class.Type, request.Priority, a.AveragePricePerAdult));

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainmentsList);

            var result = Success(entertainmentMapper);

            result.Meta = new { Count = entertainmentMapper.Count };

            return result;
        }
        #endregion
    }
}
