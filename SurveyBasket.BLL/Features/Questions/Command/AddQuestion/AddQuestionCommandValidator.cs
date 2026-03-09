namespace SurveyBasket.BLL.Features.Questions.Command.AddQuestion;

public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
{
    public AddQuestionCommandValidator()
    {
        RuleFor(x => x.pollId)
            .GreaterThan(0);

        RuleFor(x => x.question.Content)
            .NotEmpty().WithMessage("Question content must not be empty.")
            .MaximumLength(1000).WithMessage("Question content must not exceed 1000 characters.");

        RuleFor(x => x.question.Answers)
            .Must(x => x.Count > 1).WithMessage("A question must have at least two possible answers.");

        RuleFor(x => x.question.Answers)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Duplicate answers are not allowed.")
            .When(x => x.question.Answers != null);
    }
}