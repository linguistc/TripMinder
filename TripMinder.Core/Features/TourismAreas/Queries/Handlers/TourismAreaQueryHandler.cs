using AutoMapper;
using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
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
        #endregion

        #region Constructors

        public TourismAreasQueryHandler(IMapper mapper, ITourismAreaService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        #endregion

        #region Methods

        public async Task<Respond<List<GetTourismAreasListResponse>>> Handle(GetAllTourismAreasQuery request, CancellationToken cancellationToken)
        {
            var tourismAreas = await service.GetAllTourismAreasAsync();

            var tourismAreaMapper = this.mapper.Map<List<GetTourismAreasListResponse>>(tourismAreas);

            return Success(tourismAreaMapper);
        }

        public async Task<Respond<GetTourismAreaByIdResponse>> Handle(GetTourismAreaByIdQuery request, CancellationToken cancellationToken)
        {
            var tourismArea = await service.GetTourismAreaByIdWithIncludeAsync(request.Id);

            if (tourismArea == null)
                return NotFound<GetTourismAreaByIdResponse>("Object Not Found");

            var result = this.mapper.Map<GetTourismAreaByIdResponse>(tourismArea);

            return Success(result);
        }

        #endregion
    }
}