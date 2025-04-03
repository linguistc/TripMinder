using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Responses;
using TripMinder.Core.Resources;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.Entertainments.Queries.Handlers
{


    public class EntertainmentQueryHandler : RespondHandler
                                        , IRequestHandler<GetAllEntertainmentsQuery, Respond<List<GetEntertainmentsListResponse>>>
                                        , IRequestHandler<GetEntertainmentByIdQuery, Respond<GetEntertainmentByIdResponse>>
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

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetAllEntertainmentsQuery request, CancellationToken cancellationToken)
        {
            var entertainments = await service.GetAllEntertainmentsAsync();

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

        #endregion
    }
}