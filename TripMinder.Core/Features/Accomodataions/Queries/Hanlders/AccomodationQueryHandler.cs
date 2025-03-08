using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Accomodataions.Queries.Models;
using TripMinder.Core.Features.Accomodataions.Queries.Responses;
using TripMinder.Service.Contracts;
using TripMinder.Core.Resources;

namespace TripMinder.Core.Features.Accomodataions.Queries.Hanlders
{
    public class AccomodationQueryHandler : RespondHandler
                                         , IRequestHandler<GetAccomodationByIdQuery, Respond<GetAccomodationByIdResponse>>
                                         , IRequestHandler<GetAccomodationsListQuery, Respond<List<GetAccomodationsListResponse>>>
    {
        #region Fields
        private readonly IAccomodationService service;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        #endregion


        #region Constructors
        public AccomodationQueryHandler(IAccomodationService service, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer)
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
            var accomodationList = await this.service.GetAllAccomodationsAsync();

            var accomodationMapper = this.mapper.Map<List<GetAccomodationsListResponse>>(accomodationList);

            var result = Success(accomodationMapper);
            
            result.Meta = new {Count = accomodationMapper.Count};

            return result;
        }

        #endregion
    }
}
