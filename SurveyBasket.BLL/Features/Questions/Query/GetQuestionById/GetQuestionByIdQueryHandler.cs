namespace SurveyBasket.BLL.Features.Questions.Query.GetQuestionById;
public class GetQuestionByIdQueryHandler(IPollRepository pollRepository,IQuestionRepository questionRepository) : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponseDTO>>
{
    private readonly IPollRepository _pollrepository = pollRepository;
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result<QuestionResponseDTO>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {

        var isPollExist = await _pollrepository.getPollByIdAsync(request.pollId, cancellationToken);

        if (isPollExist is null)
            return Result.Failure<QuestionResponseDTO>(PollErrors.PollNotFound);

        var question = await _questionrepository.GetQuestionByIdAsync(request.pollId, request.questionId, cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponseDTO>(QuestionErrors.QuestionNotFound);

        var response = question.Adapt<QuestionResponseDTO>();

        return Result.Success(response);
    }
}
