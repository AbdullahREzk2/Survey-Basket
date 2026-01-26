namespace SurveyBasket.BLL.Contracts.Votes;
public class VoteRequestValidations:AbstractValidator<VoteRequest>
{
    public VoteRequestValidations()
    {
        RuleFor(x => x.Answers)
            .NotEmpty();

        RuleForEach(x => x.Answers)
            .SetInheritanceValidator(v =>
            v.Add(new VoteAnswerValidation())
            );
    }
}
