namespace SurveyBasket.BLL.Contracts.Votes;
public class VoteAnswerValidation:AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerValidation()
    {
        RuleFor(x=>x.QuestionId)
            .GreaterThan(0);
        RuleFor(x=>x.AnswerId)
            .GreaterThan(0);
    }
}
