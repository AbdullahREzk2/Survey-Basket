namespace SurveyBasket.BLL.Contracts.Questions;
public class QuestionRequestValidation:AbstractValidator<QuestionRequestDTO>
{
    public QuestionRequestValidation()
    {
       RuleFor(x=>x.Content)
            .NotEmpty()
            .WithMessage("Question content must not be empty.")
            .MaximumLength(1000)
            .WithMessage("Question content must not exceed 1000 characters.");

        RuleFor(x => x.Answers)
           .Must(x => x.Count > 1)
           .WithMessage("A question must have at least two possible answers.");

        RuleFor(x=>x.Answers)
            .Must(x=>x.Distinct().Count() == x.Count)
            .WithMessage("you cannot add the duplicated answers for the same Question.");

    }

}

