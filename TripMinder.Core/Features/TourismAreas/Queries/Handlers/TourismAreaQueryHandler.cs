using AutoMapper;
using MediatR;
using TripMinder.Core.Bases;
using TripMinder.Core.Features.TourismAreas.Queries.Models;
using TripMinder.Core.Features.TourismAreas.Queries.Responses;
using TripMinder.Service.Contracts;

namespace TripMinder.Core.Features.TourismAreas.Queries.Handlers
{


    public class TourismAreasQueryHandler : RespondHandler
                                        , IRequestHandler<GetAllTourismAreasQuery, Respond<List<GetAllTourismAreasResponse>>>
                                        , IRequestHandler<GetTourismAreaByIdQuery, Respond<GetSingleTourismAreaResponse>>
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

        public async Task<Respond<List<GetAllTourismAreasResponse>>> Handle(GetAllTourismAreasQuery request, CancellationToken cancellationToken)
        {
            var tourismAreas = await service.GetAllTourismAreasAsync();

            var tourismAreaMapper = this.mapper.Map<List<GetAllTourismAreasResponse>>(tourismAreas);

            return Success(tourismAreaMapper);
        }

        public async Task<Respond<GetSingleTourismAreaResponse>> Handle(GetTourismAreaByIdQuery request, CancellationToken cancellationToken)
        {
            var tourismArea = await service.GetTourismAreaByIdAsync(request.Id);

            if (tourismArea == null)
                return NotFound<GetSingleTourismAreaResponse>("Object Not Found");

            var result = this.mapper.Map<GetSingleTourismAreaResponse>(tourismArea);

            return Success(result);
        }

        #endregion
    }
}