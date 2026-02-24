namespace SurveyBasket.BLL.Service;
public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionrepository;
    private readonly IPollRepository _pollrepository;
    private readonly IVoteRepository _voterepository;

    public QuestionService(IQuestionRepository questionRepository,IPollRepository pollRepository,IVoteRepository voteRepository)
    {
        _questionrepository = questionRepository;
        _pollrepository = pollRepository;
        _voterepository = voteRepository;
    }

    public async Task<Result<PaginatedList<QuestionResponseDTO>>>GetAllQuestionsForPollAsync(int pollId,RequestFilters filters, CancellationToken cancellationToken)
    {
        var isPollExist = await _pollrepository.getPollByIdAsync(pollId,cancellationToken);

        if (isPollExist == null)
            return Result.Failure<PaginatedList<QuestionResponseDTO>>(PollErrors.PollNotFound);

        var query = _questionrepository.GetAllQuestionsForPollAsync(pollId,filters.SearchValue!);

        if (!query.Any())
            return Result.Failure<PaginatedList<QuestionResponseDTO>>(
                QuestionErrors.QuestionNotFound);

        var projectedQuery = query.ProjectToType<QuestionResponseDTO>();

        var result = await PaginatedList<QuestionResponseDTO> .CreateAsync(projectedQuery,filters.PageNumber,filters.PageSize,cancellationToken);

        return Result.Success(result);
    }
    public async Task<Result<IEnumerable<QuestionResponseDTO>>> GetAvailableQuestionsAsync(int pollId, string userId, CancellationToken cancellationToken)
    {
        var hasVote = await _voterepository.HasUserVotedAsync(pollId, userId, cancellationToken);

        if (hasVote)
            return Result.Failure<IEnumerable<QuestionResponseDTO>>(VoteErrors.UserAlreadyVoted);

        var isPollExist = await _pollrepository.getAvailblePollAsync(pollId, cancellationToken);

        if (isPollExist is null)
            return Result.Failure<IEnumerable<QuestionResponseDTO>>(PollErrors.PollNotFound);

        var questions = await _questionrepository.GetAvailbaleForPollAsync(pollId, cancellationToken);

        var questionResponses = questions.Adapt<IEnumerable<QuestionResponseDTO>>();

        return Result.Success(questionResponses);
    }
    public async Task<Result<QuestionResponseDTO>>GetQuestionByIdAsync(int pollId, int questionId, CancellationToken cancellationToken)
    {
        var isPollExist = await _pollrepository.getPollByIdAsync(pollId, cancellationToken);

        if (isPollExist is null)
            return Result.Failure<QuestionResponseDTO>(PollErrors.PollNotFound);

        var question = await _questionrepository.GetQuestionByIdAsync(pollId, questionId, cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponseDTO>(QuestionErrors.QuestionNotFound);

        var response = question.Adapt<QuestionResponseDTO>();

        return Result.Success(response);
    }
    public async Task<Result<QuestionResponseDTO>> AddQuestionAsync(int pollId,QuestionRequestDTO question,CancellationToken cancellationToken)
    {
        var poll = await _pollrepository
            .getPollByIdAsync(pollId, cancellationToken);

        if (poll is null)
            return Result.Failure<QuestionResponseDTO>(
                PollErrors.PollNotFound);

        var exists = await _questionrepository
            .searchQuestion(pollId, question.Content, cancellationToken);

        if (exists)
            return Result.Failure<QuestionResponseDTO>(
                QuestionErrors.QuestionAlreadyExists);

        var questionEntity = question.Adapt<Question>();
        questionEntity.PollId = pollId;
        questionEntity.isActive = true;
        questionEntity.answers = question.Answers
            .Select(a => new Answer { Content = a })
            .ToList();

        var createdQuestion =
            await _questionrepository.AddQuestionAsync(questionEntity, cancellationToken);

        return createdQuestion is not null
            ? Result.Success(createdQuestion.Adapt<QuestionResponseDTO>())
            : Result.Failure<QuestionResponseDTO>(
                QuestionErrors.QuestionCreationFailed);
    }
    public async Task<Result> UpdateQuestionAsync(int pollId,int questionId,QuestionRequestDTO question,CancellationToken cancellationToken)
    {
        var questionEntity = await _questionrepository
            .GetQuestionByIdAsync(pollId, questionId, cancellationToken);

        if (questionEntity is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        questionEntity.Content = question.Content;

        var incomingAnswers = question.Answers
            .Select(a => a.Trim())
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);


        // 2️⃣ Deactivate removed answers
        foreach (var answer in questionEntity.answers)
        {
            if (!incomingAnswers.Contains(answer.Content))
            {
                answer.isActive = false;
            }
        }

        // 3️⃣ Activate existing answers or add new ones
        foreach (var incomingAnswer in incomingAnswers)
        {
            var existingAnswer = questionEntity.answers
                .FirstOrDefault(a =>
                    a.Content.Equals(incomingAnswer, StringComparison.OrdinalIgnoreCase));

            if (existingAnswer is not null)
            {
                existingAnswer.isActive = true;
            }
            else
            {
                questionEntity.answers.Add(new Answer
                {
                    Content = incomingAnswer,
                    isActive = true
                });
            }
        }

        await _questionrepository.UpdateQuestionAsync(questionEntity, cancellationToken);

        return Result.Success();
    }
    public async Task<Result> activeToggleQuestion(int pollId, int questionId, CancellationToken cancellationToken)
    {
        var isUpdated = await _questionrepository.activeToggleQuestion(pollId, questionId, cancellationToken);
        
        if (isUpdated)
            return Result.Success();

        return Result.Failure(QuestionErrors.QuestionActivationFailed);
    }


}



