namespace SurveyBasket.BLL.Service;
public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardrepository;
    private readonly IPollRepository _pollrepository;

    public DashboardService(IDashboardRepository dashboardRepository,IPollRepository pollRepository)
    {
        _dashboardrepository = dashboardRepository;
        _pollrepository = pollRepository;
    }
    public async Task<Result<PollVotesResponse>>GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollTitle = await _dashboardrepository.GetPollTitleAsync(pollId, cancellationToken);
        var voterDetails = await _dashboardrepository.GetVoterDetailsAsync(pollId, cancellationToken);
        var questionAnswers = await _dashboardrepository.GetQuestionAnswersAsync(pollId, cancellationToken);

        var votes = new PollVotesResponse(
            Title: pollTitle ?? "Unknown Poll",
            Votes: voterDetails.Select(vd => new VoteResponse(
                VoterName: vd.VoterName,
                VoteDate: vd.VoteDate,
                selectedAnswers: questionAnswers
                    .Where(qa => qa.VoteId == vd.VoteId)
                    .Select(qa => new QuestionAnswerResponse(
                        qa.QuestionContent,
                        qa.AnswerContent
                    ))
            ))
        );
        return votes is null
            ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
            : Result.Success(votes);

    }
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> getVotesPerDay(int pollId, CancellationToken cancellationToken = default)
    {
       var Poll = await  _pollrepository.getPollByIdAsync(pollId, cancellationToken);
        if(Poll is null)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDayDTOs = await _dashboardrepository.GetVotesPerDayAsync(pollId, cancellationToken);

        var VotesPerDay = votesPerDayDTOs.Adapt<IEnumerable<VotesPerDayResponse>>();

        if(VotesPerDay is null)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(DashboardErrors.VotesPerDayNotFound);

        return Result.Success(VotesPerDay);
    }
    public async Task<Result<IEnumerable<QuestionVoteResponse>>> GetQuestionVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var poll = await _pollrepository.getPollByIdAsync(pollId, cancellationToken);
        if (poll is null)
            return Result.Failure<IEnumerable<QuestionVoteResponse>>(PollErrors.PollNotFound);

        var response = await _dashboardrepository.getVotesPerQuestionAsync(pollId, cancellationToken);
        var questionVotes = response.Adapt<IEnumerable<QuestionVoteResponse>>();

        if (questionVotes is null)
            return Result.Failure<IEnumerable<QuestionVoteResponse>>(DashboardErrors.QuestionVotesNotFound);

        return Result.Success(questionVotes);
    }




}
