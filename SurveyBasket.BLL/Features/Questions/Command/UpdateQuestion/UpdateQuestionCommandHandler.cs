namespace SurveyBasket.BLL.Features.Questions.Command.UpdateQuestion;
public class UpdateQuestionCommandHandler(IQuestionRepository questionRepository) : IRequestHandler<UpdateQuestionCommand, Result>
{
    private readonly IQuestionRepository _questionrepository = questionRepository;

    public async Task<Result> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {

        var questionEntity = await _questionrepository
            .GetQuestionByIdAsync(request.pollId, request.questionId, cancellationToken);

        if (questionEntity is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        questionEntity.Content = request.question.Content;

        var incomingAnswers = request.question.Answers
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
}
