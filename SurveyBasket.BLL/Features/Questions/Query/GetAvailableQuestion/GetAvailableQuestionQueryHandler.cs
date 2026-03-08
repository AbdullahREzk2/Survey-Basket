namespace SurveyBasket.BLL.Features.Questions.Query.GetAvailableQuestion;
public class GetAvailableQuestionQueryHandler(IVoteRepository voteRepository,IPollRepository pollRepository,IQuestionRepository questionRepository) : IRequestHandler<GetAvailableQuestionQuery, Result<IEnumerable<QuestionResponseDTO>>>
{
    private readonly IVoteRepository _voterepository = voteRepository;
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result<IEnumerable<QuestionResponseDTO>>> Handle(GetAvailableQuestionQuery request, CancellationToken cancellationToken)
    {

        var hasVote = await _voterepository.HasUserVotedAsync(request.pollId, request.userId, cancellationToken);

        if (hasVote)
            return Result.Failure<IEnumerable<QuestionResponseDTO>>(VoteErrors.UserAlreadyVoted);

        var isPollExist = await _pollrepository.getAvailblePollAsync(request.pollId, cancellationToken);

        if (isPollExist is null)
            return Result.Failure<IEnumerable<QuestionResponseDTO>>(PollErrors.PollNotFound);

        var questions = await _questionrepository.GetAvailbaleForPollAsync(request.pollId, cancellationToken);

        var questionResponses = questions.Adapt<IEnumerable<QuestionResponseDTO>>();

        return Result.Success(questionResponses);
    }
}
