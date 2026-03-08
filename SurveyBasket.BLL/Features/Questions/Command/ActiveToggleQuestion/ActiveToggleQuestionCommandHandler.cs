namespace SurveyBasket.BLL.Features.Questions.Command.ActiveToggleQuestion;
public class ActiveToggleQuestionCommandHandler(IQuestionRepository questionRepository) : IRequestHandler<ActiveToggleQuestionCommand, Result>
{
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result> Handle(ActiveToggleQuestionCommand request, CancellationToken cancellationToken)
    {

        var isUpdated = await _questionrepository.activeToggleQuestion(request.pollId, request.questionId, cancellationToken);

        if (isUpdated)
            return Result.Success();

        return Result.Failure(QuestionErrors.QuestionActivationFailed);
    }
}
