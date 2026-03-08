namespace SurveyBasket.BLL.Features.Questions.Command.AddQuestion;
public class AddQuestionCommandHandler(IPollRepository pollRepository,IQuestionRepository questionRepository) : IRequestHandler<AddQuestionCommand, Result<QuestionResponseDTO>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result<QuestionResponseDTO>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {

        var poll = await _pollrepository
            .getPollByIdAsync(request.pollId, cancellationToken);

        if (poll is null)
            return Result.Failure<QuestionResponseDTO>(
                PollErrors.PollNotFound);

        var exists = await _questionrepository
            .searchQuestion(request.pollId, request.question.Content, cancellationToken);

        if (exists)
            return Result.Failure<QuestionResponseDTO>(
                QuestionErrors.QuestionAlreadyExists);

        var questionEntity = request.question.Adapt<Question>();
        questionEntity.PollId = request.pollId;
        questionEntity.isActive = true;
        questionEntity.answers = request.question.Answers
            .Select(a => new Answer { Content = a })
            .ToList();

        var createdQuestion =
            await _questionrepository.AddQuestionAsync(questionEntity, cancellationToken);

        return createdQuestion is not null
            ? Result.Success(createdQuestion.Adapt<QuestionResponseDTO>())
            : Result.Failure<QuestionResponseDTO>(
                QuestionErrors.QuestionCreationFailed);
    }
}

