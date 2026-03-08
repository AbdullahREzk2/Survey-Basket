namespace SurveyBasket.BLL.Features.Polls.Queries.GetAllPolls;
public class GetAllPollsQueryHandler(IPollRepository pollRepository, IMapper mapper) : IRequestHandler<GetAllPollsQuery, Result<IEnumerable<PollResponseDTO>>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<PollResponseDTO>>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await _pollrepository.getAllPollsAsync(cancellationToken);
        var response = _mapper.Map<IEnumerable<PollResponseDTO>>(polls);
        return response is not null
            ? Result.Success(response)
            : Result.Failure<IEnumerable<PollResponseDTO>>(PollErrors.PollNotFound);
    }
}
