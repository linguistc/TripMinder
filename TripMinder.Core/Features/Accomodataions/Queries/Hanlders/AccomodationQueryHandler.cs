using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Behaviors;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;
using TripMinder.Service.Contracts;
using TripMinder.Core.Resources;

namespace TripMinder.Core.Features.Accomodataions.Queries.Hanlders
{
    public class AccomodationQueryHandler : RespondHandler
                                         , IRequestHandler<GetAccomodationByIdQuery, Respond<GetAccomodationByIdResponse>>
                                         , IRequestHandler<GetAccomodationsListQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByGovernorateIdQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByZoneIdQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByClassIdQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByTypeIdQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByRatingQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByNumberOfBedsQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListByNumberOfAdultsQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListLessThanPriceQuery, Respond<List<GetAccomodationsListResponse>>>
                                         , IRequestHandler<GetAccomodationsListMoreThanPriceQuery, Respond<List<GetAccomodationsListResponse>>>
    {
        #region Fields
        private readonly IAccomodationService service;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        #endregion


        #region Constructors
        public AccomodationQueryHandler(IAccomodationService service, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.service = service;
            this.mapper = mapper;
            this.stringLocalizer = this.stringLocalizer;
        }
        #endregion


        #region Functions
        public async Task<Respond<GetAccomodationByIdResponse>> Handle(GetAccomodationByIdQuery request, CancellationToken cancellationToken)
        {
            var accomodation = await this.service.GetAccomodationByIdWithIncludeAsync(request.Id);

            if (accomodation == null)
                return NotFound<GetAccomodationByIdResponse>(this.stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = this.mapper.Map<GetAccomodationByIdResponse>(accomodation);

            return Success(result);
        }

        async Task<Respond<List<GetAccomodationsListResponse>>> IRequestHandler<GetAccomodationsListQuery, Respond<List<GetAccomodationsListResponse>>>.Handle(GetAccomodationsListQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListAsync();

            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);
            
            result.Meta = new {Count = accomodationMapper.Count};

            return result;
        }
        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByZoneIdQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByZoneIdAsync(request.ZoneId, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }
        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByGovernorateIdQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByGovernorateIdAsync(request.GovernorateId, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }

        #endregion

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByClassIdQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByClassIdAsync(request.ClassId, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByTypeIdAsync(request.TypeId, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;        
        }

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByRatingQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByRatingAsync(request.Rating, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByNumberOfBedsQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByNumberOfBedsAsync(request.NumOfBeds, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListByNumberOfAdultsQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListByNumberOfAdultsAsync(request.NumOfAdults, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);            

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListLessThanPriceQuery request, CancellationToken cancellationToken)
        {
            
            var accomodationList = await this.service.GetAccomodationsListLessThanPriceAsync((double)request.Price, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
            
        }

        public async Task<Respond<List<GetAccomodationsListResponse>>> Handle(GetAccomodationsListMoreThanPriceQuery request, CancellationToken cancellationToken)
        {
            var accomodationList = await this.service.GetAccomodationsListMoreThanPriceAsync((double)request.Price, cancellationToken);
            
            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);

            result.Meta = new { Count = accomodationMapper.Count };

            return result;
        }
    }
}
