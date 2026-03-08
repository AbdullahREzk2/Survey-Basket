namespace SurveyBasket.BLL.Features.Votes.Command;
public class AddVoteCommandHandler(IVoteRepository voteRepository,IPollRepository pollRepository,IQuestionRepository questionRepository) : IRequestHandler<AddVoteCommand, Result>
{
    private readonly IVoteRepository _voterepository = voteRepository;
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result> Handle(AddVoteCommand request, CancellationToken cancellationToken)
    {
        var hasVoted = await _voterepository
                    .HasUserVotedAsync(request.pollId, request.userId, cancellationToken);

        if (hasVoted)
            return Result.Failure(VoteErrors.UserAlreadyVoted);

        var poll = await _pollrepository
            .getAvailblePollAsync(request.pollId, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        var questions = await _questionrepository
            .GetAvailbaleForPollAsync(request.pollId, cancellationToken);

        if (!questions.Any())
            return Result.Failure(VoteErrors.InvalidQuestions);

        if (request.voteRequest.Answers
            .GroupBy(x => x.QuestionId)
            .Any(g => g.Count() > 1))
        {
            return Result.Failure(VoteErrors.DuplicateQuestionAnswer);
        }

        var pollQuestionIds = questions
            .Select(q => q.questionId)
            .ToHashSet();

        var answeredQuestionIds =request.voteRequest.Answers
            .Select(a => a.QuestionId)
            .ToHashSet();

        if (!answeredQuestionIds.IsSubsetOf(pollQuestionIds))
            return Result.Failure(VoteErrors.InvalidQuestions);


        var validAnswersMap = questions.ToDictionary(
            q => q.questionId,
            q => q.answers.Select(a => a.answerId).ToHashSet()
        );

        foreach (var answer in request.voteRequest.Answers)
        {
            if (!validAnswersMap.TryGetValue(answer.QuestionId, out var validAnswers) ||
                !validAnswers.Contains(answer.AnswerId))
            {
                return Result.Failure(VoteErrors.InvalidAnswers);
            }
        }

        var vote = new Vote
        {
            PollId = request.pollId,
            UserId = request.userId,
            voteAnswers = request.voteRequest.Answers
                .Adapt<List<VoteAnswer>>()
        };

        var isAdded = await _voterepository
            .AddVoteAsync(vote, cancellationToken);

        if (!isAdded)
            return Result.Failure(VoteErrors.VoteNotAdded);

        return Result.Success();
    }
}
