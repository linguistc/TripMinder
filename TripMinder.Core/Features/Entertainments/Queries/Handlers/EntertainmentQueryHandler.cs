public class EntertainmentQueryHandler : 
    IRequestHandler<GetAllEntertainmentsQuery, GetAllEntertainmentsResponse>,
    IRequestHandler<GetEntertainmentByIdQuery, GetSingleEntertainmentResponse>
{
    private readonly IMapper _mapper;
    private readonly IEntertainmentRepository _entertainmentRepository;

    public EntertainmentQueryHandler(IMapper mapper, IEntertainmentRepository entertainmentRepository)
    {
        _mapper = mapper;
        _entertainmentRepository = entertainmentRepository;
    }

    public async Task<GetAllEntertainmentsResponse> Handle(GetAllEntertainmentsQuery request, CancellationToken cancellationToken)
    {
        var entertainments = await _entertainmentRepository.GetAllAsync();
        return new GetAllEntertainmentsResponse
        {
            Entertainments = _mapper.Map<List<EntertainmentDto>>(entertainments),
            Success = true
        };
    }

    public async Task<GetSingleEntertainmentResponse> Handle(GetEntertainmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entertainment = await _entertainmentRepository.GetByIdAsync(request.Id);
        return new GetSingleEntertainmentResponse
        {
            Entertainment = _mapper.Map<EntertainmentDto>(entertainment),
            Success = true
        };
    }
}