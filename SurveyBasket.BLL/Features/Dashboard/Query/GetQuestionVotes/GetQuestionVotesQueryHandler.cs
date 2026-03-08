namespace SurveyBasket.BLL.Features.Dashboard.Query.GetQuestionVotes;
public class GetQuestionVotesQueryHandler(IDashboardRepository dashboardRepository,IPollRepository pollRepository) : IRequestHandler<GetQuestionVotesQuery,Result<IEnumerable<QuestionVoteResponse>>>
{
    private readonly IDashboardRepository _dashboardrepository = dashboardRepository;
    private readonly IPollRepository _pollrepository = pollRepository;

    public async Task<Result<IEnumerable<QuestionVoteResponse>>> Handle(GetQuestionVotesQuery request, CancellationToken cancellationToken)
    {

        var poll = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);
        if (poll is null)
            return Result.Failure<IEnumerable<QuestionVoteResponse>>(PollErrors.PollNotFound);

        var response = await _dashboardrepository.getVotesPerQuestionAsync(request.pollId, cancellationToken);
        var questionVotes = response.Adapt<IEnumerable<QuestionVoteResponse>>();

        if (questionVotes is null)
            return Result.Failure<IEnumerable<QuestionVoteResponse>>(DashboardErrors.QuestionVotesNotFound);

        return Result.Success(questionVotes);
    }
}
