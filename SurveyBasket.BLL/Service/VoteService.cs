namespace SurveyBasket.BLL.Service;
public class VoteService : IVoteService
{
    private readonly IPollRepository _pollrepository;
    private readonly IVoteRepository _voterepository;
    private readonly IQuestionRepository _questionrepository;

    public VoteService (IPollRepository pollRepository,IVoteRepository voteRepository,IQuestionRepository questionRepository)
    {
        _pollrepository = pollRepository;
        _voterepository = voteRepository;
        _questionrepository = questionRepository;
    }

    public async Task<Result> AddVoteAsync(int pollId,string userId,VoteRequest voteRequest,CancellationToken cancellationToken)
    {
        var hasVoted = await _voterepository
            .HasUserVotedAsync(pollId, userId, cancellationToken);

        if (hasVoted)
            return Result.Failure(VoteErrors.UserAlreadyVoted);

        var poll = await _pollrepository
            .getAvailblePollAsync(pollId, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        var questions = await _questionrepository
            .GetAvailbaleForPollAsync(pollId, cancellationToken);

        if (!questions.Any())
            return Result.Failure(VoteErrors.InvalidQuestions);

        if (voteRequest.Answers
            .GroupBy(x => x.QuestionId)
            .Any(g => g.Count() > 1))
        {
            return Result.Failure(VoteErrors.DuplicateQuestionAnswer);
        }

        var pollQuestionIds = questions
            .Select(q => q.questionId)
            .ToHashSet();

        var answeredQuestionIds = voteRequest.Answers
            .Select(a => a.QuestionId)
            .ToHashSet();

        if (!answeredQuestionIds.IsSubsetOf(pollQuestionIds))
            return Result.Failure(VoteErrors.InvalidQuestions);


        var validAnswersMap = questions.ToDictionary(
            q => q.questionId,
            q => q.answers.Select(a => a.answerId).ToHashSet()
        );

        foreach (var answer in voteRequest.Answers)
        {
            if (!validAnswersMap.TryGetValue(answer.QuestionId, out var validAnswers) ||
                !validAnswers.Contains(answer.AnswerId))
            {
                return Result.Failure(VoteErrors.InvalidAnswers);
            }
        }

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            voteAnswers = voteRequest.Answers
                .Adapt<List<VoteAnswer>>()
        };

        var isAdded = await _voterepository
            .AddVoteAsync(vote, cancellationToken);

        if (!isAdded)
            return Result.Failure(VoteErrors.VoteNotAdded);

        return Result.Success();
    }

}
