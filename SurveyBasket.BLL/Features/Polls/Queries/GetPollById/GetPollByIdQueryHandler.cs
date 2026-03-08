namespace SurveyBasket.BLL.Features.Polls.Queries.GetPollById;
public class GetPollByIdQueryHandler(IPollRepository pollRepository,IMapper mapper) : IRequestHandler<GetPollByIdQuery, Result<PollResponseDTO>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<PollResponseDTO>> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var poll = await _pollrepository.getPollByIdAsync(request.PollId, cancellationToken);
        var response = _mapper.Map<PollResponseDTO>(poll);
        return response is not null
            ? Result.Success(response)
            : Result.Failure<PollResponseDTO>(PollErrors.PollNotFound);
    }
}
