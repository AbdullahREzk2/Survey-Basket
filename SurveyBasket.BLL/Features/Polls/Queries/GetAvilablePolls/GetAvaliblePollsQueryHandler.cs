namespace SurveyBasket.BLL.Features.Polls.Queries.GetAvilablePolls;
public class GetAvaliblePollsQueryHandler(IPollRepository pollRepository) : IRequestHandler<GetAvaliblePollsQuery, Result<IEnumerable<PollResponseDTO>>>
{
    private readonly IPollRepository _pollrepository = pollRepository;

    public async Task<Result<IEnumerable<PollResponseDTO>>> Handle(GetAvaliblePollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await _pollrepository.getAvailblePollsAsync(cancellationToken);
        var response = polls.Adapt<IEnumerable<PollResponseDTO>>();
        return response is not null
            ? Result.Success(response)
            : Result.Failure<IEnumerable<PollResponseDTO>>(PollErrors.PollNotFound);
    }

}
