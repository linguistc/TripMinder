using AutoMapper;
using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.Entertainments.Queries.Models;
using TripMinder.Core.Features.Entertainments.Queries.Responses;
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
        #endregion

        #region Constructors

        public EntertainmentQueryHandler(IMapper mapper, IEntertainmentService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        #endregion

        #region Methods

        public async Task<Respond<List<GetEntertainmentsListResponse>>> Handle(GetAllEntertainmentsQuery request, CancellationToken cancellationToken)
        {
            var entertainments = await service.GetAllEntertainmentsAsync();

            var entertainmentMapper = this.mapper.Map<List<GetEntertainmentsListResponse>>(entertainments);

            return Success(entertainmentMapper);
        }

        public async Task<Respond<GetEntertainmentByIdResponse>> Handle(GetEntertainmentByIdQuery request, CancellationToken cancellationToken)
        {
            var entertainment = await service.GetEntertainmentByIdWithIncludeAsync(request.Id);

            if (entertainment == null)
                return NotFound<GetEntertainmentByIdResponse>("Object Not Found");

            var result = this.mapper.Map<GetEntertainmentByIdResponse>(entertainment);

            return Success(result);
        }

        #endregion
    }
}