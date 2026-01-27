namespace SurveyBasket.BLL.IService;
public interface IDashboardService
{
    Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<VotesPerDayResponse>>> getVotesPerDay(int pollId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionVoteResponse>>> GetQuestionVotesAsync(int pollId, CancellationToken cancellationToken = default);
}
