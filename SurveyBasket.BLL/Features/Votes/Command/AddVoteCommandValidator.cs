namespace SurveyBasket.BLL.Features.Votes.Command;

public class AddVoteCommandValidator : AbstractValidator<AddVoteCommand>
{
    public AddVoteCommandValidator()
    {
        RuleFor(x => x.pollId)
            .GreaterThan(0);

        RuleFor(x => x.userId)
            .NotEmpty();

        RuleFor(x => x.voteRequest.Answers)
            .NotEmpty()
            .WithMessage("At least one answer is required");

        RuleForEach(x => x.voteRequest.Answers)
            .ChildRules(answer =>
            {
                answer.RuleFor(x => x.QuestionId)
                    .GreaterThan(0);
                answer.RuleFor(x => x.AnswerId)
                    .GreaterThan(0);
            });
    }
}