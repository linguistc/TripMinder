public class TourismAreaQueryHandler : 
    IRequestHandler<GetAllTourismAreasQuery, GetAllTourismAreasResponse>,
    IRequestHandler<GetTourismAreaByIdQuery, GetSingleTourismAreaResponse>
{
    private readonly IMapper _mapper;
    private readonly ITourismAreaRepository _tourismAreaRepository;

    public TourismAreaQueryHandler(IMapper mapper, ITourismAreaRepository tourismAreaRepository)
    {
        _mapper = mapper;
        _tourismAreaRepository = tourismAreaRepository;
    }

    public async Task<GetAllTourismAreasResponse> Handle(GetAllTourismAreasQuery request, CancellationToken cancellationToken)
    {
        var tourismAreas = await _tourismAreaRepository.GetAllAsync();
        return new GetAllTourismAreasResponse
        {
            TourismAreas = _mapper.Map<List<TourismAreaDto>>(tourismAreas),
            Success = true
        };
    }

    public async Task<GetSingleTourismAreaResponse> Handle(GetTourismAreaByIdQuery request, CancellationToken cancellationToken)
    {
        var tourismArea = await _tourismAreaRepository.GetByIdAsync(request.Id);
        return new GetSingleTourismAreaResponse
        {
            TourismArea = _mapper.Map<TourismAreaDto>(tourismArea),
            Success = true
        };
    }
}